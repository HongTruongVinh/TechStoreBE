using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Extensions;
using TechStore.Common.Helpers;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Payment;
using TechStore.Model.DTOs.Snapshot;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;
        private readonly PaymentSettings _paymentSettings;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork uow,
            ILogger<OrderService> logger,
            SequenceGeneratorService sequenceService,
            IOptions<PaymentSettings> paymentSettings
            )
        {
            _uow = uow;
            _sequenceService = sequenceService;
            _logger = logger;
            _paymentSettings = paymentSettings.Value;
        }

        public async Task<ServiceResult<CreatePaymentSnapshotResult>> CreateSnapshotAsync(string userId, OrderCreateModel orderCreateModel, string idempotencyKey)
        {

            var customer = await _uow.Users.TableNoTracking.Where(u => u.PublicId == userId).FirstOrDefaultAsync();

            if (customer == null)
            {
                return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.NotFound, Messenger.NotFoundUser);
            }

            var requestHash = ShareFunctions.ComputeHash(orderCreateModel);
            var transaction = await _uow.BeginTransactionAsync();
            try
            {
                #region validate idempotency key
                var existingIdempotencyKey = await _uow.IdempotencyKeys.TableNoTracking.FirstOrDefaultAsync(x =>
                                                        x.UserId == customer.Id &&
                                                        x.RequestKey == idempotencyKey);

                if (existingIdempotencyKey != null)
                {
                    // Check body request hash to ensure the same request is being made
                    if (existingIdempotencyKey.RequestHash == requestHash)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Success(JsonSerializer.Deserialize<CreatePaymentSnapshotResult>(existingIdempotencyKey.ResponseBody));
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.IdempotencyKeyConflict, Messenger.IdempotencyKeyConflict);
                    }
                }
                #endregion

                #region Validate Items

                Guid snapshotId = Guid.NewGuid();
                string snapshotPublicId = _sequenceService.GetNextSnapshotId();


                List<PaymentSnapshotItem> snapshotItems = new List<PaymentSnapshotItem>();
                var productVariantOptionsDic = new Dictionary<ProductVariantOption, int>();

                // Tính toán tổng tiền
                decimal subtotalAmount = 0;

                var groupedItems = orderCreateModel.Items
                                    .GroupBy(x => x.ProductVariantOptionId)
                                    .Select(g => new
                                    {
                                        ProductVariantOptionId = g.Key,
                                        Quantity = g.Sum(x => x.Quantity)
                                    })
                                    .ToList();

                foreach (var item in groupedItems)
                {
                    var pVO = await _uow.ProductVariantOptions.GetForUpdateAsync_PostgreSQL(item.ProductVariantOptionId);

                    if (pVO == null)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.NotFound, Messenger.NoExitData + " " + item.ProductVariantOptionId);
                    }

                    if (item.Quantity <= 0)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.BadRequest, Messenger.BadRequest);
                    }

                    var reservedStock = await _uow.StockReservations
                                                .TableNoTracking
                                                .Where(x =>
                                                    x.ProductVariantOptionId == pVO.Id &&
                                                    x.Status == StockReservationStatus.Reserved &&
                                                    x.ExpiresAt > TimeZoneHelper.GetUtcNow())
                                                .SumAsync(x => x.Quantity);

                    if (item.Quantity > pVO.Stock - reservedStock)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.ConfictData, OrderMessenger.NotEnoughQuantity);
                    }

                    var productVariant = await _uow.ProductVariants
                        .TableNoTracking
                        .Where(x => x.Id == pVO.ProductVariantId)
                        .Include(x => x.Product)
                        .ThenInclude(p => p.Category)
                        .FirstOrDefaultAsync();

                    if (productVariant == null)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.Status500InternalServerError, Messenger.SystemError);
                    }

                    snapshotItems.Add(new PaymentSnapshotItem
                    {
                        Id = Guid.NewGuid(),
                        PublicId = ShareFunctions.GenerateRandomStringId(),
                        PaymentSnapshotId = snapshotId,
                        ProductVariantOptionId = pVO.Id,
                        ProductVariantOptionPublicId = pVO.PublicId,

                        CategoryName = productVariant.Product.Category.Name,
                        ProductName = productVariant.Product.Name + " " + productVariant.Name + " " + pVO.Name,
                        UrlImage = pVO.ImageUrl,

                        Quantity = item.Quantity,
                        PriceAtOrderTime = pVO.Price,
                        TotalPrice = item.Quantity * pVO.Price,

                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        UpdatedAt = TimeZoneHelper.GetUtcNow(),
                        CreatedBy = customer.Id,
                    });

                    productVariantOptionsDic.Add(pVO, item.Quantity);

                    decimal itemTotal = item.Quantity * pVO.Price;
                    subtotalAmount += itemTotal;
                }

                if (snapshotItems.Count < 1)
                {
                    transaction.Rollback();
                    return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.BadRequest, Messenger.BadRequest);
                }

                #endregion

                #region Validate Voucher
                Voucher? voucher = null;
                decimal discountedAmount = 0;

                if (!string.IsNullOrWhiteSpace(orderCreateModel.VoucherCode))
                {
                    voucher = await _uow.Vouchers.GetForUpdateByVoucherCodeAsync_PostgreSQL(orderCreateModel.VoucherCode);

                    if (voucher == null)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.NotFound, VoucherMessenger.VoucherNotFound);
                    }

                    if (voucher.EndDate < DateTime.UtcNow)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherExpired);
                    }

                    if (voucher.StartDate > DateTime.UtcNow)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherExpired);
                    }

                    if (voucher.Status != EVoucherStatus.Active)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherExpired);
                    }

                    if (voucher.Available <= 0)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherUsageExceeded);
                    }

                    var usageCount = await _uow.VoucherUsages.CountAsync(x => x.UserId == customer.Id && x.VoucherId == voucher.Id);

                    if (usageCount >= voucher.UsageLimit)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherUsageExceeded);
                    }

                    if (subtotalAmount < voucher.MinOrderPrice)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.ConfictData, VoucherMessenger.MinOrderPriceNotMet);
                    }

                    if (voucher.DiscountType == EDiscountType.Percentage)
                    {
                        discountedAmount = voucher.DiscountValue * subtotalAmount;

                        if (discountedAmount > voucher.MaxDiscountAmount) discountedAmount = voucher.MaxDiscountAmount;
                    }
                    else
                    {
                        discountedAmount = voucher.DiscountValue;
                    }

                    voucher.ReservedCount++;
                }

                #endregion

                decimal shippingCharge = 0;
                var totalAmount = subtotalAmount - discountedAmount + shippingCharge;

                if (totalAmount < 0)
                {
                    totalAmount = 0;
                }

                #region Create snapshot

                var snapshot = new PaymentSnapshot
                {
                    Id = snapshotId,
                    PublicId = snapshotPublicId,
                    CustomerId = customer.Id,
                    CustomerName = orderCreateModel.CustomerName,
                    CustomerEmail = orderCreateModel.CustomerEmail,
                    ShippingAddress = orderCreateModel.ShippingAddress,
                    CustomerPhoneNumber = orderCreateModel.CustomerPhoneNumber,
                    Note = orderCreateModel.Note,
                    VoucherId = voucher != null ? voucher.Id : null,

                    ExpiredAt = TimeZoneHelper.GetUtcNow().AddMinutes(_paymentSettings.ExpireMinutes),
                    Status = totalAmount == 0 ? EPaymentSnapshotStatus.Paid : EPaymentSnapshotStatus.PendingPayment,

                    SubtotalAmount = subtotalAmount,
                    ShippingCharge = shippingCharge,
                    TotalAmount = totalAmount,
                    DiscountAmount = discountedAmount,

                    Items = snapshotItems,

                    CreatedAt = TimeZoneHelper.GetUtcNow(),
                    UpdatedAt = TimeZoneHelper.GetUtcNow(),
                    CreatedBy = customer.Id,
                };

                await _uow.PaymentSnapshots.AddAsync(snapshot);


                foreach (var item in productVariantOptionsDic)
                {
                    await _uow.StockReservations.AddAsync(new StockReservation
                    {
                        ProductVariantOptionId = item.Key.Id,
                        PaymentSnapshotId = snapshotId,
                        Quantity = item.Value,
                        ReservedAt = TimeZoneHelper.GetUtcNow(),
                        ExpiresAt = snapshot.ExpiredAt,

                        PublicId = ShareFunctions.GenerateRandomStringId(),
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                    });
                }

                var createPaymentSnapshotResult = new CreatePaymentSnapshotResult
                {
                    SnapshotId = snapshot.PublicId,
                    Amount = snapshot.TotalAmount,
                    Status = snapshot.Status
                };

                if (existingIdempotencyKey == null)
                {
                    var idempotencyKeyId = Guid.NewGuid();

                    var idempotencyKeyEntity = new IdempotencyKey
                    {
                        Id = idempotencyKeyId,
                        PublicId = ShareFunctions.GenerateRandomStringId(),
                        UserId = customer.Id,
                        Endpoint = "/api/order/create-snapshot",
                        StatusCode = 200,
                        ExpiredAt = TimeZoneHelper.GetUtcNow().AddHours(24),
                        RequestKey = idempotencyKey,
                        RequestHash = requestHash,
                        ResponseBody = JsonSerializer.Serialize(createPaymentSnapshotResult),
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                    };

                    await _uow.IdempotencyKeys.AddAsync(idempotencyKeyEntity);
                }

                #endregion

                #region Add new order if final amount == 0
                if(totalAmount == 0)
                {
                    Guid orderId = Guid.NewGuid();
                    var order = new Order
                    {
                        Id = orderId,
                        CustomerId = customer.Id,
                        CustomerPublicId = customer.PublicId,
                        CustomerName = orderCreateModel.CustomerName,
                        CustomerPhoneNumber = orderCreateModel.CustomerPhoneNumber,
                        CustomerEmail = orderCreateModel.CustomerEmail,
                        ShippingAddress = orderCreateModel.ShippingAddress,

                        SubtotalAmount = snapshot.SubtotalAmount,
                        DiscountAmount = snapshot.DiscountAmount,
                        ShippingCharge = snapshot.ShippingCharge,
                        TotalAmount = snapshot.TotalAmount,

                        OrderStatus = EOrderStatus.Processing,
                        OrderItems = snapshotItems.Select(x => x.ToOrderItem(orderId)).ToList(),

                        PublicId = await _sequenceService.GetNextOrderIdAsync(),
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                    };

                    var invoice = new Invoice
                    {
                        Id = Guid.NewGuid(),
                        PublicId = await _sequenceService.GetNextInvoiceIdAsync(),

                        SubTotal = order.SubtotalAmount,
                        TotalAmount = order.TotalAmount,
                        DiscountAmount= order.DiscountAmount,
                        PaidAmount = order.TotalAmount,

                        Payments = new List<Payment>(),

                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        CreatedBy = customer.Id,
                        InvoiceStatus = EInvoiceStatus.Paid,
                    };

                    Payment payment = new Payment
                    {
                        Id = Guid.NewGuid(),
                        PublicId = await _sequenceService.GetNextPaymentIdAsync(),
                        Invoice = invoice,
                        InvoiceId = invoice.Id,
                        User = customer,
                        UserId = customer.Id,
                        Amount = totalAmount,
                        PaymentMethod = EPaymentMethod.VoucherOrFree,
                        PaymentStatus = EPaymentStatus.Paid,
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                    };

                    invoice.Payments.Add(payment);
                    order.Invoice = invoice;

                    await _uow.Orders.AddAsync(order);

                    createPaymentSnapshotResult.OrderId = order.PublicId;
                }

                #endregion

                var result = await _uow.CommitAsync();

                if (result < 1)
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<CreatePaymentSnapshotResult>.Fail(EErrorType.SystemError, Messenger.SystemError);
                }

                await transaction.CommitAsync();

                return ServiceResult<CreatePaymentSnapshotResult>.Success(createPaymentSnapshotResult);
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();

                if (ex.InnerException is not PostgresException postgresException ||
                    postgresException.SqlState != PostgresErrorCodes.UniqueViolation ||
                    postgresException.ConstraintName != "IX_IdempotencyKeys_UserId_RequestKey")
                {
                    throw;
                }

                var existingIdempotencyKey = await _uow.IdempotencyKeys.TableNoTracking
                    .SingleOrDefaultAsync(x =>
                        x.UserId == customer.Id &&
                        x.RequestKey == idempotencyKey);

                if (existingIdempotencyKey == null)
                {
                    throw;
                }

                if (existingIdempotencyKey.RequestHash != requestHash)
                {
                    return ServiceResult<CreatePaymentSnapshotResult>.Fail(
                        EErrorType.IdempotencyKeyConflict,
                        Messenger.IdempotencyKeyConflict);
                }

                var storedResponse = JsonSerializer.Deserialize<CreatePaymentSnapshotResult>(
                    existingIdempotencyKey.ResponseBody);

                if (storedResponse == null)
                {
                    throw new InvalidOperationException("Stored idempotency response is invalid.");
                }

                return ServiceResult<CreatePaymentSnapshotResult>.Success(storedResponse);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ServiceResult<CreatePrePayOnlineOrderResult>> CreatePrepaidOnlineOrderFromSepayWebhookAsync(SepayWebhookRequest request)
        {
            var paymentTransaction = new PaymentTransaction()
            {
                TransactionId = request.Id.ToString(),
                PaymentCode = request.Code,
                Amount = request.TransferAmount,
                TransactionDate = request.TransactionDate,
                ReferenceCode = request.ReferenceCode,
                Gateway = request.Gateway,
                TransferContent = request.Content,
                Status = EPaymentTransactionStatus.Received,

                PublicId = ShareFunctions.GenerateRandomStringId(),
                CreatedAt = TimeZoneHelper.GetUtcNow()
            };

            await using var transaction = await _uow.BeginTransactionAsync();

            try
            {
                //Validate duplicate transaction
                var existingTransaction = await _uow.PaymentTransactions.FindOneAsync(
                    x => x.TransactionId == paymentTransaction.TransactionId);

                if (existingTransaction != null)
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<CreatePrePayOnlineOrderResult>.Fail(EErrorType.ConfictData, PaymentMessenger.PaymentAlreadyProcessed);
                }

                var snapshot = await _uow.PaymentSnapshots.GetForUpdateAsync_PostgreSQL(request.Code);
                if (snapshot == null)
                {
                    _logger.LogInformation(OrderMessenger.NotFoundSnapshot + " snapshot Id " + request.Code);
                    paymentTransaction.Status = EPaymentTransactionStatus.InvalidSnapshot;
                    await _uow.PaymentTransactions.AddAsync(paymentTransaction);
                    await _uow.CommitAsync();
                    await transaction.CommitAsync();
                    return ServiceResult<CreatePrePayOnlineOrderResult>.Fail(EErrorType.NotFound, Messenger.SystemError);
                }

                if (snapshot.Status != EPaymentSnapshotStatus.PendingPayment || snapshot.ExpiredAt <= TimeZoneHelper.GetUtcNow())
                {
                    paymentTransaction.Status = EPaymentTransactionStatus.ExpiredSnapshot;
                    await _uow.PaymentTransactions.AddAsync(paymentTransaction);
                    await _uow.CommitAsync();
                    await transaction.CommitAsync();
                    return ServiceResult<CreatePrePayOnlineOrderResult>.Fail(EErrorType.ConfictData, PaymentMessenger.PaymentSnapshotExpired);
                }

                var customer = await _uow.Users.GetByInternalIdAsync(snapshot.CustomerId);
                if (customer == null)
                {
                    _logger.LogInformation(Messenger.NotFoundUser + " user Id " + snapshot.CustomerId);
                    paymentTransaction.PaymentSnapshotId = snapshot.Id;
                    paymentTransaction.Status = EPaymentTransactionStatus.ManualReview;
                    paymentTransaction.Note = Messenger.NotFoundUser;
                    await _uow.PaymentTransactions.AddAsync(paymentTransaction);
                    await _uow.CommitAsync();
                    await transaction.CommitAsync();
                    return ServiceResult<CreatePrePayOnlineOrderResult>.Fail(EErrorType.NotFound, Messenger.SystemError);
                }

                if (request.TransferAmount < snapshot.TotalAmount)
                {
                    paymentTransaction.PaymentSnapshotId = snapshot.Id;
                    paymentTransaction.Status = EPaymentTransactionStatus.IncorrectAmount;
                    paymentTransaction.Note = PaymentMessenger.IncorrectAmount;
                    await _uow.PaymentTransactions.AddAsync(paymentTransaction);
                    await _uow.CommitAsync();
                    await transaction.CommitAsync();
                    return ServiceResult<CreatePrePayOnlineOrderResult>.Fail(EErrorType.ConfictData, PaymentMessenger.IncorrectAmount);
                }

                if (snapshot.TotalAmount < request.TransferAmount)
                {
                    customer.WalletBalance += request.TransferAmount - snapshot.TotalAmount;
                    _uow.Users.Update(customer);
                }
                else if (snapshot.TotalAmount > request.TransferAmount)
                {
                    _logger.LogInformation(PaymentMessenger.IncorrectAmount + " sanpshot Id " + request.Code);
                    paymentTransaction.Status = EPaymentTransactionStatus.IncorrectAmount;
                    paymentTransaction.Note = PaymentMessenger.IncorrectAmount;
                    await _uow.PaymentTransactions.AddAsync(paymentTransaction);
                    await _uow.CommitAsync();
                    await transaction.CommitAsync();
                    return ServiceResult<CreatePrePayOnlineOrderResult>.Fail(EErrorType.ConfictData, PaymentMessenger.IncorrectAmount);
                }

                var snapshotItems = await _uow.PaymentSnapshotItems.FindManyAsync(i => i.PaymentSnapshotId == snapshot.Id);
                snapshot.Items = snapshotItems.ToList();

                paymentTransaction.PaymentSnapshotId = snapshot.Id;

                #region Validate StockReservation

                var reservations = await _uow.StockReservations.GetByPaymentSnapshotIdAsync(snapshot.Id, CancellationToken.None);
                if (reservations.Count == 0 || reservations.Any(x => x.Status != StockReservationStatus.Reserved))
                {
                    paymentTransaction.Status = EPaymentTransactionStatus.ManualReview;
                    paymentTransaction.Note = OrderMessenger.NotEnoughQuantity;
                    await _uow.PaymentTransactions.AddAsync(paymentTransaction);
                    await _uow.CommitAsync();
                    await transaction.CommitAsync();
                    return ServiceResult<CreatePrePayOnlineOrderResult>.Fail(EErrorType.ConfictData, OrderMessenger.NotEnoughQuantity);
                }

                var reservationQuantities = reservations
                    .GroupBy(x => x.ProductVariantOptionId)
                    .ToDictionary(x => x.Key, x => x.Sum(r => r.Quantity));

                if (snapshotItems.Any(item =>
                        !reservationQuantities.TryGetValue(item.ProductVariantOptionId, out var quantity) ||
                        quantity != item.Quantity) ||
                    reservationQuantities.Count != snapshotItems.Count)
                {
                    paymentTransaction.Status = EPaymentTransactionStatus.ManualReview;
                    paymentTransaction.Note = "Stock reservation does not match payment snapshot.";
                    await _uow.PaymentTransactions.AddAsync(paymentTransaction);
                    await _uow.CommitAsync();
                    await transaction.CommitAsync();
                    return ServiceResult<CreatePrePayOnlineOrderResult>.Fail(EErrorType.ConfictData, Messenger.SystemError);
                }

                foreach (var item in snapshotItems)
                {
                    var pvo = await _uow.ProductVariantOptions.GetForUpdateAsync_PostgreSQL(item.ProductVariantOptionPublicId);
                    if (pvo == null || pvo.Stock < item.Quantity)
                    {
                        paymentTransaction.Status = EPaymentTransactionStatus.ManualReview;
                        paymentTransaction.Note = OrderMessenger.NotEnoughQuantity;
                        await _uow.PaymentTransactions.AddAsync(paymentTransaction);
                        await _uow.CommitAsync();
                        await transaction.CommitAsync();
                        return ServiceResult<CreatePrePayOnlineOrderResult>.Fail(EErrorType.ConfictData, OrderMessenger.NotEnoughQuantity);
                    }

                    pvo.Stock -= item.Quantity;
                    _uow.ProductVariantOptions.Update(pvo);
                }

                foreach (var reservation in reservations)
                {
                    reservation.Status = StockReservationStatus.Confirmed;
                    _uow.StockReservations.Update(reservation);
                }

                #endregion

                #region Validate Voucher
                Voucher? voucher = null;

                if (snapshot.VoucherId != null)
                {
                    voucher = await _uow.Vouchers.GetForUpdateByIdAsync_PostgreSQL(snapshot.VoucherId.Value);

                    if (voucher == null)
                    {
                        paymentTransaction.PaymentSnapshotId = snapshot.Id;
                        paymentTransaction.Status = EPaymentTransactionStatus.ManualReview;
                        paymentTransaction.Note = "Voucher not found.";

                        await _uow.PaymentTransactions.AddAsync(paymentTransaction);
                        await _uow.CommitAsync();
                        await transaction.CommitAsync();

                        return ServiceResult<CreatePrePayOnlineOrderResult>.Fail(EErrorType.SystemError, Messenger.SystemError);
                    }
                }

                #endregion

                #region create order

                Guid orderId = Guid.NewGuid();
                string orderPublicId = await _sequenceService.GetNextOrderIdAsync();

                var order = new Order
                {
                    Id = orderId,
                    CustomerId = customer.Id,
                    CustomerPublicId = customer.PublicId,
                    CustomerName = snapshot.CustomerName,
                    CustomerPhoneNumber = snapshot.CustomerPhoneNumber,
                    CustomerEmail = snapshot.CustomerEmail,
                    ShippingAddress = snapshot.ShippingAddress,
                    Note = snapshot.Note,

                    SubtotalAmount = snapshot.SubtotalAmount,
                    DiscountAmount = snapshot.DiscountAmount,
                    ShippingCharge = snapshot.ShippingCharge,
                    TotalAmount = snapshot.TotalAmount,

                    OrderStatus = EOrderStatus.Processing,
                    OrderItems = snapshotItems.Select(x => x.ToOrderItem(orderId)).ToList(),

                    PublicId = orderPublicId,
                    CreatedAt = TimeZoneHelper.GetUtcNow()
                };

                var invoice = new Invoice
                {
                    Id = Guid.NewGuid(),
                    PublicId = await _sequenceService.GetNextInvoiceIdAsync(),

                    SubTotal = order.SubtotalAmount,
                    TotalAmount = order.TotalAmount,
                    DiscountAmount = order.DiscountAmount,
                    PaidAmount = order.TotalAmount,

                    Payments = new List<Payment>(),

                    CreatedAt = TimeZoneHelper.GetUtcNow(),
                    CreatedBy = customer.Id,
                    InvoiceStatus = EInvoiceStatus.Paid
                };

                Payment payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    PublicId = await _sequenceService.GetNextPaymentIdAsync(),
                    Invoice = invoice,
                    InvoiceId = invoice.Id,
                    User = customer,
                    UserId = customer.Id,
                    Amount = order.TotalAmount,
                    PaymentMethod = EPaymentMethod.DomesticBank,
                    PaymentStatus = EPaymentStatus.Paid,
                    CreatedAt = TimeZoneHelper.GetUtcNow()
                };

                paymentTransaction.PaymentId = payment.Id;
                paymentTransaction.Status = EPaymentTransactionStatus.Processed;
                await _uow.PaymentTransactions.AddAsync(paymentTransaction);

                invoice.Payments.Add(payment);
                order.Invoice = invoice;

                await _uow.Orders.AddAsync(order);

                // Add VoucherUsage
                if (voucher != null)
                {
                    voucher.UsedCount += 1;
                    voucher.ReservedCount -= 1;
                    _uow.Vouchers.Update(voucher);

                    await _uow.VoucherUsages.AddAsync(
                        new VoucherUsage
                        {
                            PublicId = ShareFunctions.GenerateRandomStringId(),
                            VoucherId = voucher.Id,
                            UserId = customer.Id,
                            OrderId = order.Id,
                            UsedAt = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow
                        });
                }

                snapshot.Status = EPaymentSnapshotStatus.Paid;
                snapshot.PaidAt = TimeZoneHelper.GetUtcNow();
                snapshot.UpdatedAt = TimeZoneHelper.GetUtcNow();
                _uow.PaymentSnapshots.Update(snapshot);

                var result = await _uow.CommitAsync();

                if (result < 1)
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<CreatePrePayOnlineOrderResult>.Fail(EErrorType.SystemError, Messenger.SystemError);
                }

                #endregion

                await transaction.CommitAsync();

                var createOrderResult = new CreatePrePayOnlineOrderResult
                {
                    OrderId = order.PublicId,
                    PaymentSnapshotId = snapshot.PublicId,
                    Amount = order.TotalAmount,
                };

                return ServiceResult<CreatePrePayOnlineOrderResult>.Success(createOrderResult);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ServiceResult<CreateCODOnlineOrderResult>> CreateCODOnlineOrderAsync(string userId, OrderCreateModel orderCreateModel, string idempotencyKey)
        {
            var customer = await _uow.Users.GetByIdAsync(userId);
            if (customer == null)
            {
                return ServiceResult<CreateCODOnlineOrderResult>.Fail(EErrorType.NotFound, Messenger.NotFoundUser);
            }

            if (orderCreateModel.Items == null || orderCreateModel.Items.Count == 0)
            {
                return ServiceResult<CreateCODOnlineOrderResult>.Fail(EErrorType.BadRequest, Messenger.BadRequest);
            }

            var requestHash = ShareFunctions.ComputeHash(orderCreateModel);

            await using var transaction = await _uow.BeginTransactionAsync();

            try
            {
                #region validate idempotency key
                var existingIdempotencyKey = await _uow.IdempotencyKeys.TableNoTracking.FirstOrDefaultAsync(x =>
                                                        x.UserId == customer.Id &&
                                                        x.RequestKey == idempotencyKey);

                if (existingIdempotencyKey != null)
                {
                    // Check body request hash to ensure the same request is being made
                    if (existingIdempotencyKey.RequestHash == requestHash)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreateCODOnlineOrderResult>.Success(JsonSerializer.Deserialize<CreateCODOnlineOrderResult>(existingIdempotencyKey.ResponseBody));
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreateCODOnlineOrderResult>.Fail(EErrorType.IdempotencyKeyConflict, Messenger.IdempotencyKeyConflict);
                    }
                }
                #endregion

                var groupedItems = orderCreateModel.Items
                    .GroupBy(x => x.ProductVariantOptionId)
                    .OrderBy(x => x.Key, StringComparer.Ordinal)
                    .Select(x => new
                    {
                        ProductVariantOptionId = x.Key,
                        Quantity = x.Sum(item => item.Quantity)
                    })
                    .ToList();

                if (groupedItems.Any(x => string.IsNullOrWhiteSpace(x.ProductVariantOptionId) || x.Quantity <= 0))
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<CreateCODOnlineOrderResult>.Fail(EErrorType.BadRequest, Messenger.BadRequest);
                }

                var orderItems = new List<OrderItem>();
                var productVariantOptionsDic = new Dictionary<ProductVariantOption, int>();
                var orderId = Guid.NewGuid();
                decimal subtotalAmount = 0;
                var now = TimeZoneHelper.GetUtcNow();

                foreach (var item in groupedItems)
                {
                    var pVO = await _uow.ProductVariantOptions
                        .GetForUpdateAsync_PostgreSQL(item.ProductVariantOptionId);

                    if (pVO == null)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreateCODOnlineOrderResult>.Fail(EErrorType.NotFound, Messenger.NoExitData);
                    }

                    var reservedStock = await _uow.StockReservations.TableNoTracking
                        .Where(x => x.ProductVariantOptionId == pVO.Id &&
                                    x.Status == StockReservationStatus.Reserved &&
                                    x.ExpiresAt > now)
                        .SumAsync(x => x.Quantity);

                    if (item.Quantity > pVO.Stock - reservedStock)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreateCODOnlineOrderResult>.Fail(EErrorType.ConfictData, OrderMessenger.NotEnoughQuantity);
                    }

                    var productVariant = await _uow.ProductVariants.TableNoTracking
                        .Where(x => x.Id == pVO.ProductVariantId)
                        .Include(x => x.Product)
                        .ThenInclude(p => p.Category)
                        .FirstOrDefaultAsync();

                    if (productVariant == null)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreateCODOnlineOrderResult>.Fail(EErrorType.Status500InternalServerError, Messenger.SystemError);
                    }

                    orderItems.Add(new OrderItem
                    {
                        PublicId = $"{now:yyyyMMdd}{Random.Shared.Next(100000, 1000000):D6}",
                        ProductVariantOptionId = pVO.Id,
                        ProductVariantOptionPublicId = pVO.PublicId,
                        OrderId = orderId,
                        CategoryName = productVariant.Product.Category.Name,
                        ProductName = productVariant.Product.Name + " " + productVariant.Name + " " + pVO.Name,
                        ImageUrl = pVO.ImageUrl,
                        Quantity = item.Quantity,
                        PriceAtOrderTime = pVO.Price,
                        TotalPrice = item.Quantity * pVO.Price,
                        CreatedAt = now
                    });

                    productVariantOptionsDic.Add(pVO, item.Quantity);

                    subtotalAmount += item.Quantity * pVO.Price;
                }

                Voucher? voucher = null;
                decimal discountAmount = 0;

                if (!string.IsNullOrWhiteSpace(orderCreateModel.VoucherCode))
                {
                    voucher = await _uow.Vouchers
                        .GetForUpdateByVoucherCodeAsync_PostgreSQL(orderCreateModel.VoucherCode);

                    if (voucher == null ||
                        voucher.Status != EVoucherStatus.Active ||
                        voucher.StartDate > now ||
                        voucher.EndDate < now ||
                        voucher.Available <= 0 ||
                        subtotalAmount < voucher.MinOrderPrice)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreateCODOnlineOrderResult>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherExpired);
                    }

                    var usageCount = await _uow.VoucherUsages.CountAsync(
                        x => x.UserId == customer.Id && x.VoucherId == voucher.Id);

                    if (usageCount >= voucher.UsageLimit)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<CreateCODOnlineOrderResult>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherUsageExceeded);
                    }

                    discountAmount = voucher.DiscountType == EDiscountType.Percentage
                        ? voucher.DiscountValue * subtotalAmount
                        : voucher.DiscountValue;

                    if (discountAmount > voucher.MaxDiscountAmount)
                    {
                        discountAmount = voucher.MaxDiscountAmount;
                    }
                }

                var totalAmount = Math.Max(0, subtotalAmount - discountAmount);
                var order = new Order
                {
                    Id = orderId,
                    PublicId = await _sequenceService.GetNextOrderIdAsync(),
                    Customer = customer,
                    CustomerId = customer.Id,
                    CustomerPublicId = customer.PublicId,
                    CustomerName = orderCreateModel.CustomerName,
                    CustomerPhoneNumber = orderCreateModel.CustomerPhoneNumber,
                    CustomerEmail = orderCreateModel.CustomerEmail,
                    SubtotalAmount = subtotalAmount,
                    ShippingCharge = 0,
                    DiscountAmount = discountAmount,
                    TotalAmount = totalAmount,
                    OrderStatus = EOrderStatus.Pending,
                    OrderItems = orderItems,
                    ShippingAddress = orderCreateModel.ShippingAddress,
                    Note = orderCreateModel.Note,
                    CreatedAt = now,
                    UpdatedAt = now,
                    CreatedBy = customer.Id
                };

                await _uow.Orders.AddAsync(order);

                foreach (var item in productVariantOptionsDic)
                {
                    item.Key.Stock -= item.Value;
                    item.Key.SoldCount += item.Value;
                    _uow.ProductVariantOptions.Update(item.Key);
                }

                if (voucher != null)
                {
                    voucher.UsedCount++;
                    _uow.Vouchers.Update(voucher);

                    await _uow.VoucherUsages.AddAsync(new VoucherUsage
                    {
                        PublicId = ShareFunctions.GenerateRandomStringId(),
                        VoucherId = voucher.Id,
                        UserId = customer.Id,
                        OrderId = order.Id,
                        UsedAt = now,
                        CreatedAt = now
                    });
                }

                var createCODOnlineOrderResult = new CreateCODOnlineOrderResult { Id = order.PublicId };

                if (existingIdempotencyKey == null)
                {
                    var idempotencyKeyId = Guid.NewGuid();

                    var idempotencyKeyEntity = new IdempotencyKey
                    {
                        Id = idempotencyKeyId,
                        PublicId = ShareFunctions.GenerateRandomStringId(),
                        UserId = customer.Id,
                        Endpoint = "/api/order/create-cod-order",
                        StatusCode = 200,
                        ExpiredAt = TimeZoneHelper.GetUtcNow().AddHours(24),
                        RequestKey = idempotencyKey,
                        RequestHash = requestHash,
                        ResponseBody = JsonSerializer.Serialize(createCODOnlineOrderResult),
                        CreatedAt = TimeZoneHelper.GetUtcNow()
                    };

                    await _uow.IdempotencyKeys.AddAsync(idempotencyKeyEntity);
                }

                var result = await _uow.CommitAsync();
                if (result < 1)
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<CreateCODOnlineOrderResult>.Fail(EErrorType.Status500InternalServerError, Messenger.SystemError);
                }

                await transaction.CommitAsync();

                return ServiceResult<CreateCODOnlineOrderResult>.Success(createCODOnlineOrderResult);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

       
        public async Task<ServiceResult<string>> CreateInStoreOrderAsync(string userId, string paymentId, InStoreOrderCreateModel orderCreateModel)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.BadRequest,
            };

            //var cashier = await _uow.Users.GetByIdAsync(userId);
            //if (cashier == null)
            //{
            //    serviceResult.Message = Messenger.NoExitData + " " + userId;
            //    return serviceResult;
            //}



            //var newUserId = await _sequenceService.GetNextUserIdAsync();

            //var newUser = new User
            //{
            //    PublicId = newUserId,
            //    FirstName = orderCreateModel.FirstName,
            //    LastName = orderCreateModel.LastName,
            //    PhoneNumber = orderCreateModel.CustomerPhonenumber,
            //    Email = orderCreateModel.CustomerEmail ?? "",
            //    PasswordHash = orderCreateModel.CustomerPhonenumber,
            //    Address = "",
            //    RoleId = ERole.Customer,
            //    CreatedAt = TimeZoneHelper.GetUtcNow(),
            //    CreatedBy = cashier.Id,
            //    UpdatedAt = TimeZoneHelper.GetUtcNow(),
            //    EntityStatus = EEntityStatus.Active,
            //    Status = EUserStatus.Active,
            //};

            //await _uow.Users.AddAsync(newUser);
            //await _uow.CommitAsync();

            //var orderId = await _sequenceService.GetNextOrderIdAsync();
            //var invoiceId = await _sequenceService.GetNextInvoiceIdAsync();
            ////var paymentId = await _sequenceService.GetNextPaymentIdAsync();

            //// Tính toán tổng tiền
            //decimal totalPrice = 0;
            //decimal totalDiscount = 0;

            //var order = new Order
            //{
            //    PublicId = orderId,
            //    OrderType = EOrderType.InStore,
            //    CustomerId = newUser.Id,
            //    Customer = newUser,
            //    CustomerName = orderCreateModel.LastName + " " + orderCreateModel.FirstName,
            //    CustomerPhoneNumber = orderCreateModel.CustomerPhonenumber,
            //    CustomerEmail = orderCreateModel.CustomerEmail ?? "",
            //    ShippingAddress = "Nhận tại của hàng",
            //    TotalPrice = 0,
            //    ShippingCharge = 0,
            //    DiscountAmount = totalDiscount,
            //    FinalAmount = 0,
            //    PaymentMethod = orderCreateModel.PaymentMethod,
            //    OrderStatus = EOrderStatus.Pending,
            //    OrderItems = new List<OrderItem>(),
            //    Payments = new List<Payment>(),

            //    CreatedAt = TimeZoneHelper.GetUtcNow(),
            //    CreatedBy = cashier.Id,
            //    UpdatedAt = TimeZoneHelper.GetUtcNow(),
            //    EntityStatus = EEntityStatus.Active,
            //};

            //await _uow.Orders.AddAsync(order);
            //await _uow.CommitAsync();

            //List<OrderItem> orderItems = new List<OrderItem>();
            //foreach (var item in orderCreateModel.Items)
            //{
            //    var productVariantOption = await _uow.ProductVariantOptions.GetProductVariantOptionDetailByPublicIdAsync(item.ProductVariantOptionId);

            //    if (productVariantOption == null)
            //    {
            //        serviceResult.Message = Messenger.NoExitData + " " + item.ProductVariantOptionId;
            //        return serviceResult;
            //    }

            //    if (productVariantOption.Stock < item.Quantity)
            //    {
            //        serviceResult.Message = Messenger.NoExitData + " " + item.ProductVariantOptionId;
            //        return serviceResult;
            //    }
            //    else
            //    {
            //        productVariantOption.Stock -= item.Quantity;
            //        _uow.ProductVariantOptions.Update(productVariantOption);

            //        orderItems.Add(new OrderItem
            //        {
            //            PublicId = Random.Shared.Next(100000, 1000000).ToString(),
            //            ProductVariantOptionId = productVariantOption.Id,
            //            ProductVariantOptionPublicId = productVariantOption.PublicId,
            //            Order = order,
            //            OrderId = order.Id,
            //            Quantity = item.Quantity,
            //            PriceAtOrderTime = productVariantOption.ProductVariant.Price,
            //            Discount = item.Discount,
            //            TotalPrice = item.Quantity * productVariantOption.ProductVariant.Price,
            //            CreatedAt = TimeZoneHelper.GetUtcNow(),
            //            EntityStatus = EEntityStatus.Active,
            //        });
            //    }

            //    decimal itemTotal = item.Quantity * productVariantOption.ProductVariant.Price;
            //    totalPrice += itemTotal;
            //    totalDiscount += item.Discount;
            //}

            //await _uow.OrderItems.AddRangeAsync(orderItems);

            //var finalAmount = totalPrice - totalDiscount;

            //var invoice = new Invoice
            //{
            //    PublicId = invoiceId,
            //    OrderId = order.Id,
            //    Order = order,
            //    CreatedAt = TimeZoneHelper.GetUtcNow(),
            //    TotalPrice = totalPrice,
            //    DiscountAmount = 0,
            //    FinalAmount = finalAmount,
            //    InvoiceStatus = EInvoiceStatus.Unpaid,
            //    EntityStatus = EEntityStatus.Active,
            //};
            //await _uow.Invoices.AddAsync(invoice);

            //var payment = new Payment
            //{
            //    PublicId = paymentId,
            //    OrderId = order.Id,
            //    Order = order,
            //    PaymentMethod = orderCreateModel.PaymentMethod,
            //    Amount = finalAmount,
            //    TransactionCode = "",
            //    PaymentStatus = EPaymentStatus.Pending,
            //    CreatedAt = TimeZoneHelper.GetUtcNow(),
            //    EntityStatus = EEntityStatus.Active,
            //};
            //await _uow.Payments.AddAsync(payment);

            //string paymentQR = ServerAddress.WEBSITE_ADDRESS + payment.PublicId;
            //var addPaymentQRResult = await _qrCodeService.AddPaymentQRCodeAsync(payment, paymentQR, paymentId, EQRCodeType.Payment, null);

            //if (addPaymentQRResult.IsSuccess == false)
            //{
            //    serviceResult.Message = Messenger.SystemError;
            //    return serviceResult;
            //}

            //order.Invoice = invoice;
            //order.Payments.Add(payment);
            //order.TotalPrice = totalPrice;
            //order.DiscountAmount = totalDiscount;
            //order.FinalAmount = finalAmount;

            //_uow.Orders.Update(order);

            //var result = await _uow.CommitAsync();

            //if (result < 1)
            //{
            //    return serviceResult;
            //}

            //serviceResult.Data = order.PublicId;
            //serviceResult.IsSuccess = true;
            //serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> DeleteOrderAsync(string orderId)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var order = await _uow.Orders.GetByIdAsync(orderId);
            if (order == null)
            {
                return serviceResult;
            }

            _uow.OrderItems.RemoveRange(order.OrderItems);

            if (order.Invoice != null)
            {
                var invoice = await _uow.Invoices.FindOneAsync(i => i.Id == order.Invoice.Id);

                if (invoice != null)
                {
                    foreach (var payment in invoice.Payments)
                    {
                        var existPayment = await _uow.Payments.FindOneAsync(p => p.Id == payment.Id);
                        if (existPayment != null)
                        {
                            _uow.Payments.Remove(existPayment);
                        }
                    }

                    _uow.Invoices.Remove(invoice);
                }
            }

            _uow.Orders.Remove(order);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.Data = true;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }


        public async Task<ServiceResult<PagedResult<OrderDetailResponseModel>>> GetListOrdersByStatusIdAsync(EOrderStatus statusId, int page, int pageSize)
        {
            var serviceResult = new ServiceResult<PagedResult<OrderDetailResponseModel>>
            {
                IsSuccess = true,
                Data = new PagedResult<OrderDetailResponseModel>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = await _uow.Orders.TableNoTracking.CountAsync(o => o.OrderStatus == statusId),
                },
                Message = Messenger.GetDataSuccessful,
            };

            var listOrders = await _uow.Orders.TableNoTracking.Where(o => o.OrderStatus == statusId)
                .Include(o => o.OrderItems)
                .Include(o => o.Invoice)
                    .ThenInclude(i => i.Payments)
                .Include(o => o.ShippingDetail)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var order in listOrders)
            {
                serviceResult.Data.Items.Add(order.ToOrderDetailModel());
            }

            return serviceResult;
        }

        public async Task<ServiceResult<PagedResult<ListItemOrderModel>>> GetOrdersAsync(OrderSearchQuery query)
        {
            var serviceResult = new ServiceResult<PagedResult<ListItemOrderModel>>
            {
                IsSuccess = true,
                Data = new PagedResult<ListItemOrderModel>
                {
                    CurrentPage = query.Page,
                    PageSize = query.PageSize,
                    TotalItems = 0,
                },
                Message = Messenger.GetDataSuccessful,
            };

            var pagedResult = await _uow.Orders.SearchAsync(query);

            foreach (var order in pagedResult.Items)
            {
                serviceResult.Data.Items.Add(order.ToListItemOrderModel());
            }

            serviceResult.Data.TotalItems = pagedResult.TotalItems;
            return serviceResult;
        }

        public async Task<ServiceResult<List<ListItemOrderModel>>> GetInStoreOrdersAsync()
        {
            var serviceResult = new ServiceResult<List<ListItemOrderModel>>
            {
                IsSuccess = true,
                Data = new List<ListItemOrderModel>(),
                Message = Messenger.GetDataSuccessful,
            };

            var listOrders = await _uow.Orders.TableNoTracking.ToListAsync();

            foreach (var order in listOrders)
            {
                serviceResult.Data.Add(order.ToListItemOrderModel());
            }

            return serviceResult;
        }

        public async Task<ServiceResult<OrderDetailResponseModel>> GetOrderByIdAsync(string userId, string orderId)
        {
            var serviceResult = new ServiceResult<OrderDetailResponseModel>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.NoExitData,
            };

            var order = await _uow.Orders.GetWithItemsAndInvoiceAsync(orderId);

            if (order == null)
            {
                return serviceResult;
            }

            var customer = await _uow.Users.GetByIdAsync(userId);
            if (customer == null)
            {
                serviceResult.Message = Messenger.NoExitData + " " + userId;
                return serviceResult;
            }

            if (customer.Id != order.CustomerId)
            {
                return serviceResult;
            }

            order.ShippingDetail = await _uow.ShippingDetails.FindOneAsync(s => s.OrderId == order.Id);
            if (order.ShippingDetail != null)
            {
                var shipper = await _uow.Shippers.FindOneAsync(s => s.Id == order.ShippingDetail.ShipperId);
                if (shipper != null)
                {
                    order.ShippingDetail.Shipper = shipper;
                }
            }

            serviceResult.Data = order.ToOrderDetailModel();
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;
            return serviceResult;
        }

        public async Task<ServiceResult<OrderDetailResponseModel>> AdminGetOrderByIdAsync(string userId, string orderId)
        {
            var serviceResult = new ServiceResult<OrderDetailResponseModel>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.NoExitData,
            };

            var adminUser = await _uow.Users.GetByIdAsync(userId);
            if (adminUser != null)
            {
                if (adminUser.RoleId != ERole.Admin)
                {
                    serviceResult.Message = Messenger.NoPermission;
                    return serviceResult;
                }
            }
            else
            {
                serviceResult.Message = Messenger.NoExitData + " " + userId;
                return serviceResult;
            }

            var order = await _uow.Orders.GetWithItemsAndInvoiceAsync(orderId);

            if (order == null)
            {
                return serviceResult;
            }

            if (order.OrderStatus == EOrderStatus.Delivering || order.OrderStatus == EOrderStatus.Completed)
            {
                order.ShippingDetail = await _uow.ShippingDetails.Table.Where(s => s.OrderId == order.Id).FirstOrDefaultAsync();
            }

            serviceResult.Data = order.ToOrderDetailModel();
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;
            return serviceResult;
        }

        #region Update Online Order Status
        public async Task<ServiceResult<bool>> UpdateOrderStatusToProcessingAsync(string updateByUserId, string orderId)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var order = await _uow.Orders.GetByIdAsync(orderId);

            var userUpdating = await _uow.Users.GetByIdAsync(updateByUserId);

            if (order == null || userUpdating == null)
            {
                return serviceResult;
            }

            var invoice = new Invoice
            {
                PublicId = await _sequenceService.GetNextInvoiceIdAsync(),
                OrderId = order.Id,
                Order = order,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                CreatedBy = userUpdating.Id,
                SubTotal = order.SubtotalAmount,
                DiscountAmount = order.DiscountAmount,
                TotalAmount = order.TotalAmount,
                PaidAmount = 0,
                Payments = new List<Payment>(),
                InvoiceStatus = EInvoiceStatus.Unpaid,
            };


            order.Invoice = invoice;
            order.OrderStatus = EOrderStatus.Processing;
            order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            order.UpdatedBy = userUpdating.Id;

            _uow.Orders.Update(order);

            var result = await _uow.CommitAsync();
            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateOrderStatusToDeliveringAsync(string updateByUserId, string orderId, UpdateOrderToDeliveringModel model)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var order = await _uow.Orders.GetByIdAsync(orderId);
            var userUpdating = await _uow.Users.GetByIdAsync(updateByUserId);
            var shipper = await _uow.Shippers.GetByIdAsync(model.ShipperId);

            if (order == null || userUpdating == null || shipper == null)
            {
                return serviceResult;
            }

            var shippingId = _uow.ShippingDetails.CountAsync();

            var shippingDetail = new ShippingDetail
            {
                PublicId = Random.Shared.Next(100000, 1000000).ToString(),
                OrderId = order.Id,
                Order = order,
                OrderPublicId = order.PublicId,
                ShipperId = shipper.Id,
                Shipper = shipper,
                ShipperPublicId = shipper.PublicId,
                ShipperName = shipper.Name,
                ShippedDate = DateTime.UtcNow,
                TrackingNumber = Random.Shared.Next(1001, 10000).ToString(),
                Status = EShippingStatus.Shipping,
                EstimatedArrival = DateTime.UtcNow.AddDays(Random.Shared.Next(3, 8)),
                FailureCount = 0,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
            };

            await _uow.ShippingDetails.AddAsync(shippingDetail);

            order.ShippingDetail = shippingDetail;
            order.OrderStatus = EOrderStatus.Delivering;
            order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            order.UpdatedBy = userUpdating.Id;

            _uow.Orders.Update(order);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateOrderStatusToCompletedAsync(string updateByUserId, string orderId)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var order = await _uow.Orders.Table.Where(o => o.PublicId == orderId).Include(o => o.OrderItems).FirstOrDefaultAsync();
            var userUpdating = await _uow.Users.GetByIdAsync(updateByUserId);

            if (order == null || userUpdating == null)
            {
                return serviceResult;
            }

            var invoice = await _uow.Invoices.FindOneAsync(i => i.OrderId == order.Id);
            if (invoice != null)
            {
                if (invoice.InvoiceStatus != EInvoiceStatus.Paid)
                {
                    serviceResult.Message = Messenger.InvoiceUnpaid;
                    return serviceResult;
                }
            }

            foreach(var item in order.OrderItems)
            {
                //var pvo = await _uow.ProductVariantOptions.GetByInternalIdAsync(item.ProductVariantOptionId);
                var pvo = await _uow.ProductVariantOptions.Table.Where(p => p.Id == item.ProductVariantOptionId)
                                                                .Include(p => p.ProductVariant).ThenInclude(p => p.Product).FirstAsync();
                if(pvo != null)
                {
                    pvo.SoldCount += 1;
                    _uow.ProductVariantOptions.Update(pvo);

                    pvo.ProductVariant.Product.SoldCount += 1;
                    _uow.Products.Update(pvo.ProductVariant.Product);
                }
            }

            order.OrderStatus = EOrderStatus.Completed;
            order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            order.UpdatedBy = userUpdating.Id;

            _uow.Orders.Update(order);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> CancelOrderByAdminAsync(string adminId, string orderId, CancelOrderModel orderUpdateStatusModel)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var order = await _uow.Orders.GetByIdAsync(orderId);
            var admin = await _uow.Users.GetByIdAsync(adminId);

            if (order == null || admin == null)
            {
                return serviceResult;
            }

            order.OrderStatus = EOrderStatus.Canceled;
            order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            order.Note += " - " + TimeZoneHelper.GetUtcNow().ToString() + OrderMessenger.Canceled + orderUpdateStatusModel.Reason;
            order.UpdatedBy = admin.Id;

            _uow.Orders.Update(order);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> CancelOrderByCustomerAsync(string userId, string orderId, CancelOrderModel orderUpdateStatusModel)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var order = await _uow.Orders.GetByIdAsync(orderId);
            var user = await _uow.Users.GetByIdAsync(userId);

            if (order == null || user == null)
            {
                return serviceResult;
            }

            if (order.CustomerId != user.Id)
            {
                return serviceResult;
            }

            order.OrderStatus = EOrderStatus.Canceled;
            order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            order.Note += " - " + TimeZoneHelper.GetUtcNow().ToString() + OrderMessenger.Canceled + orderUpdateStatusModel.Reason;
            order.UpdatedBy = user.Id;

            _uow.Orders.Update(order);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateOrderStatusToRefundedAsync(string updateByUserId, string orderId, OrderUpdateStatusModel orderUpdateStatusModel)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var order = await _uow.Orders.GetByIdAsync(orderId);
            var userUpdating = await _uow.Users.GetByIdAsync(updateByUserId);

            if (order == null || userUpdating == null)
            {
                return serviceResult;
            }

            order.OrderStatus = EOrderStatus.Refunded;
            order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            order.Note += " - " + TimeZoneHelper.GetUtcNow().ToString() + ": Reason refund: " + orderUpdateStatusModel.Note;
            order.UpdatedBy = userUpdating.Id;

            _uow.Orders.Update(order);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateOrderStatusToFailedAsync(string updateByUserId, string orderId, OrderUpdateStatusModel orderUpdateStatusModel)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var order = await _uow.Orders.GetByIdAsync(orderId);
            var userUpdating = await _uow.Users.GetByIdAsync(updateByUserId);

            if (order == null || userUpdating == null)
            {
                return serviceResult;
            }

            order.OrderStatus = EOrderStatus.Failed;
            order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            order.Note += " - " + TimeZoneHelper.GetUtcNow().ToString() + ": Reason fail: " + orderUpdateStatusModel.Note;
            order.UpdatedBy = userUpdating.Id;

            _uow.Orders.Update(order);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;
            return serviceResult;
        }
        #endregion

        public Task<ServiceResult<bool>> UpdateInStoreOrderAsync(string updatedByCashierId, string orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<InStoreOrderResponseModel>> GetInStoreOrderAsync(string id)
        {
            var serviceResult = new ServiceResult<InStoreOrderResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.NoExitData,
            };

            //var order = await _uow.Orders.GetOrderIncludeItemsAsync(o => o.PublicId == id);

            //if (order == null)
            //{
            //    return serviceResult;
            //}

            //var payment = await _uow.Payments.FindOneAsync(p => p.OrderId == order.Id);

            //if (payment == null)
            //{
            //    return serviceResult;
            //}

            //if (order.OrderStatus == EOrderStatus.Completed)
            //{
            //    serviceResult.Data = order.ToInStoreOrderResponseModel(order.OrderItems.ToList(), null, payment);
            //}
            //else
            //{
            //    var paymentQRCode = await _uow.QRCodes.FindOneAsync(q => q.RelatedId == payment.Id && q.Type == EQRCodeType.Payment);

            //    if (order.OrderItems.ToList().Count < 1 || paymentQRCode == null)
            //    {
            //        return serviceResult;
            //    }

            //    serviceResult.Data = order.ToInStoreOrderResponseModel(order.OrderItems.ToList(), paymentQRCode, payment);
            //}


            //serviceResult.IsSuccess = true;
            //serviceResult.Message = Messenger.GetDataSuccessful;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> CheckoutInStoreOrderAsync(string id)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NoExitData,
            };

            //var order = await _uow.Orders.GetByIdAsync(id);

            //if (order == null)
            //{
            //    return serviceResult;
            //}

            //var invoice = await _uow.Invoices.FindOneAsync(i => i.OrderId == order.Id);

            //var payment = await _uow.Payments.FindOneAsync(p => p.OrderId == order.Id);

            //order.OrderStatus = EOrderStatus.Completed;
            //order.UpdatedAt = TimeZoneHelper.GetUtcNow();

            //if (invoice != null)
            //{
            //    invoice.InvoiceStatus = EInvoiceStatus.Paid;
            //    invoice.UpdatedAt = TimeZoneHelper.GetUtcNow();
            //    invoice.PaidAt = TimeZoneHelper.GetUtcNow();
            //    _uow.Invoices.Update(invoice);
            //}

            //if (payment != null)
            //{
            //    payment.PaymentStatus = EPaymentStatus.Paid;
            //    payment.UpdatedAt = TimeZoneHelper.GetUtcNow();
            //    payment.TransactionCode = Random.Shared.Next(100000, 999999).ToString();
            //    _uow.Payments.Update(payment);
            //}

            //var result = await _uow.CommitAsync();
            //if (result < 1)
            //{
            //    return serviceResult;
            //}

            //serviceResult.IsSuccess = true;
            //serviceResult.Message = Messenger.GetDataSuccessful;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> ConfirmInStoreOrder(string id)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NoExitData,
            };

            //var order = await _uow.Orders.GetByIdAsync(id);

            //if (order == null)
            //{
            //    return serviceResult;
            //}

            //var invoice = await _uow.Invoices.FindOneAsync(i => i.OrderId == order.Id);

            //var payment = await _uow.Payments.FindOneAsync(p => p.OrderId == order.Id);


            //order.OrderStatus = EOrderStatus.Completed;
            //order.PaymentMethod = EPaymentMethod.Cash;
            //order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            //_uow.Orders.Update(order);

            //if (invoice != null)
            //{
            //    invoice.InvoiceStatus = EInvoiceStatus.Paid;
            //    invoice.PaidAt = TimeZoneHelper.GetUtcNow();
            //    invoice.UpdatedAt = TimeZoneHelper.GetUtcNow();
            //    _uow.Invoices.Update(invoice);
            //}

            //if (payment != null)
            //{
            //    payment.PaymentStatus = EPaymentStatus.Paid;
            //    payment.UpdatedAt = TimeZoneHelper.GetUtcNow();
            //    payment.PaymentMethod = EPaymentMethod.Cash;
            //    _uow.Payments.Update(payment);
            //}

            //var result = await _uow.CommitAsync();
            //if (result < 1)
            //{
            //    return serviceResult;
            //}

            //serviceResult.Data = true;
            //serviceResult.IsSuccess = true;
            //serviceResult.Message = Messenger.GetDataSuccessful;
            return serviceResult;
        }

        public async Task<ServiceResult<List<ListItemOrderModel>>> GetCustomerOrdersAsync(string customerId, int page, int pageSize)
        {
            var serviceResult = new ServiceResult<List<ListItemOrderModel>>
            {
                IsSuccess = true,
                Data = new List<ListItemOrderModel>(),
                Message = Messenger.GetDataSuccessful,
            };

            var customer = await _uow.Users.GetByIdAsync(customerId);

            if (customer == null)
            {
                serviceResult.IsSuccess = false;
                serviceResult.Message = Messenger.NotFoundUser;
                return serviceResult;
            }

            var listOrders = await _uow.Orders.GetOrdersIncludeItemsDetailAsync(o => o.CustomerId == customer.Id, page, pageSize);

            if (listOrders == null)
            {
                return serviceResult;
            }

            foreach (var order in listOrders)
            {
                var orderResponseModel = order.ToListItemOrderModel();
                serviceResult.Data.Add(orderResponseModel);
            }

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateOrderByCustomerAsync(string userId, string orderId, UpdateOrderModel model)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.UpdateDataError
            };

            var order = await _uow.Orders.GetByIdAsync(orderId);
            var user = await _uow.Users.GetByIdAsync(userId);

            if (order == null || user == null)
            {
                return serviceResult;
            }

            if (order.CustomerId != user.Id)
            {
                return serviceResult;
            }

            order.CustomerName = model.CustomerName;
            order.CustomerPhoneNumber = model.CustomerPhoneNumber;
            order.CustomerEmail = model.CustomerEmail;
            order.ShippingAddress = model.ShippingAddress;
            order.Note = model.Note;

            order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            order.UpdatedBy = user.Id;

            _uow.Orders.Update(order);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;
            return serviceResult;
        }

    }
}
