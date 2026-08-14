using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Options;
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
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;
        private readonly IVietQrService _vietQrService;
        private readonly IOrderService _orderService;
        private readonly PaymentSettings _paymentSettings;

        public PaymentService(IUnitOfWork uow,
            SequenceGeneratorService sequenceService,
            IVietQrService vietQrService,
            IOrderService orderService,
            IOptions<PaymentSettings> paymentSettings
            )
        {
            _uow = uow;
            _sequenceService = sequenceService;
            _vietQrService = vietQrService;
            _orderService = orderService;
            _paymentSettings = paymentSettings.Value;
        }

        public async Task<ServiceResult<string>> AddCashPaymentByAdminAsync(string cashierId, CashPaymentCreateModel cashPayment)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.NoExitData,
            };

            var invoice = await _uow.Invoices.Table
                                    .Where(i => i.PublicId == cashPayment.InvoiceId)
                                    .Include(i => i.Payments)
                                    .FirstOrDefaultAsync();
            if(invoice == null) return serviceResult;

            var customer = await _uow.Users.GetByIdAsync(cashPayment.CustomerId);
            if(customer == null) return serviceResult;

            var cashier = await _uow.Users.GetByIdAsync(cashierId);
            if (cashier == null) return serviceResult;

            var payment = new Payment()
            {
                PublicId = await _sequenceService.GetNextPaymentIdAsync(),
                Invoice = invoice,
                InvoiceId = invoice.Id,
                User = customer,
                UserId = customer.Id,
                Amount = cashPayment.Amount,
                PaymentMethod = EPaymentMethod.Cash,
                PaymentCode = "",
                TransactionId = "",
                BankReferenceCode = "",
                PaymentStatus = EPaymentStatus.Paid,
               

                EntityStatus = EEntityStatus.Active,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                CreatedBy = cashier.Id
            };

            
            await _uow.Payments.AddAsync(payment);
            
            invoice.PaidAmount += payment.Amount;

            if (invoice.PaidAmount == invoice.TotalAmount)
            {
                invoice.InvoiceStatus = EInvoiceStatus.Paid;
            }
            else if (invoice.PaidAmount < invoice.TotalAmount)
            {
                invoice.InvoiceStatus = EInvoiceStatus.PartiallyPaid;
            }

            _uow.Invoices.Update(invoice);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;
            serviceResult.Data = payment.PublicId;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdatePayment(string paymentId, PaymentUpdateModel paymentModel)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.BadRequest,
            };

            var payment = await _uow.Payments.GetByIdAsync(paymentId);

            if (payment == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }
            payment.Amount = paymentModel.Amount;
            payment.UpdatedAt = TimeZoneHelper.GetUtcNow();

            _uow.Payments.Update(payment);
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

        public async Task<ServiceResult<bool>> DeletePayment(string paymentId)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.BadRequest,
            };

            var payment = await _uow.Payments.GetByIdAsync(paymentId);
            if (payment == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            //_uow.Payments.Remove(payment);

            payment.EntityStatus = EEntityStatus.Deleted;
            _uow.Payments.Update(payment);

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

        public async Task<ServiceResult<List<PaymentResponseModel>>> GetPayments()
        {
            var serviceResult = new ServiceResult<List<PaymentResponseModel>>
            {
                IsSuccess = true,
                Data = new List<PaymentResponseModel>(),
                Message = Messenger.GetDataSuccessful,
            };

            var payments = await _uow.Payments.GetAllAsync();

            foreach (var payment in payments) 
            {
                serviceResult.Data.Add(payment.ToPaymentResponseModel());
            }

            return serviceResult;
        }

        public async Task<ServiceResult<PaymentResponseModel>> GetPayment(string paymentId)
        {
            var serviceResult = new ServiceResult<PaymentResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.NoExitData,
            };

            var payment = await _uow.Payments.GetByIdAsync(paymentId);

            if (payment == null)
            {
                return serviceResult;
            }

            serviceResult.Data = payment.ToPaymentResponseModel();
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;

            return serviceResult;
        }


        public async Task<ServiceResult<VerifyResult>> VerifyPaymentForSnapshotAsync(SepayWebhookRequest request)
        {

            var snapshot = await _uow.PaymentSnapshots.GetWithItemsAsync(request.Code);
            if (snapshot == null)
            {
                return ServiceResult<VerifyResult>.Fail(EErrorType.NotFound, Messenger.SystemError);
            }

            var user = await _uow.Users.GetByInternalIdAsync(snapshot.CustomerId);
            if (user == null)
            {
                return ServiceResult<VerifyResult>.Fail(EErrorType.NotFound, Messenger.NotFoundUser);
            }

            var transaction = await _uow.BeginTransactionAsync();

            if (snapshot.FinalAmount < request.TransferAmount)
            {
                user.WalletBalance += request.TransferAmount;
                _uow.Users.Update(user);

                var updateUserResult = await _uow.CommitAsync();
                if (updateUserResult < 1)
                {
                    return ServiceResult<VerifyResult>.Fail(EErrorType.SystemError, Messenger.SystemError);
                }

                return ServiceResult<VerifyResult>.Fail(EErrorType.ConfictData, PaymentMessenger.IncorrectAmount);
            }
            

            var paymentData = new PaymentForSnapshot
            {
                Amount = request.TransferAmount,
                Code = request.Code,
                BankReferenceCode = request.ReferenceCode,
                TransactionId = request.Id.ToString(),
            };


            var orderServiceResultOrder = await _orderService.CreatePrePayOnlineOrderAsync(user.PublicId, snapshot, paymentData);

            if (orderServiceResultOrder.IsSuccess == false)
            {
                await transaction.RollbackAsync();
                return ServiceResult<VerifyResult>.Fail(EErrorType.SystemError, Messenger.SystemError);
            }

            return ServiceResult<VerifyResult>.Success(new VerifyResult()
            {
                SnapshotId = snapshot.PublicId,
                Amount = request.TransferAmount,
                Message = PaymentMessenger.PaymentVerified
            });
        }

        public async Task<ServiceResult<PaymentDataForSnapshotModel>> CreatePaymentForSnapshotAsync(string userId, OrderCreateModel orderCreateModel, string idempotencyKey)
        {
            var customer = await _uow.Users.TableNoTracking.Where(u => u.PublicId == userId).FirstOrDefaultAsync();

            if (customer == null)
            {
                return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.NotFound, Messenger.NotFoundUser);
            }

            // validate idempotency key
            var existingIdempotencyKey = await _uow.IdempotencyKeys.TableNoTracking.Where(i => i.RequestKey == idempotencyKey).FirstOrDefaultAsync();

            if (existingIdempotencyKey != null)
            {
                // Check body request hash to ensure the same request is being made
                if (ShareFunctions.ComputeHash(existingIdempotencyKey.RequestHash) == ShareFunctions.ComputeHash(orderCreateModel))
                {
                    return ServiceResult<PaymentDataForSnapshotModel>.Success(JsonSerializer.Deserialize<PaymentDataForSnapshotModel>(existingIdempotencyKey.ResponseBody));
                }
            }

            // Validate Voucher
            Voucher? voucher = null;
            decimal discountedAmount = 0;

            if (!string.IsNullOrWhiteSpace(orderCreateModel.VoucherCode))
            {
                voucher = await _uow.Vouchers.FindOneAsync(x => x.Code == orderCreateModel.VoucherCode);

                if (voucher == null)
                {
                    return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.NotFound, VoucherMessenger.VoucherNotFound);
                }

                if (voucher.EndDate < DateTime.UtcNow)
                {
                    return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherExpired);
                }

                if (voucher.Available == 0)
                {
                    return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherUsageExceeded);
                }

                var usageCount = await _uow.VoucherUsages.CountAsync(x => x.UserId == customer.Id && x.VoucherId == voucher.Id);

                if (usageCount >= voucher.UsageLimit)
                {
                    return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.ConfictData, VoucherMessenger.VoucherUsageExceeded);
                }
            }

            var paymentId = await _sequenceService.GetNextPaymentIdAsync();

            // Tính toán tổng tiền
            decimal totalPrice = 0;
            decimal shippingCharge = 0;

            var snapshot = new PaymentSnapshot
            {
                Id = Guid.NewGuid(),
                PublicId = _sequenceService.GetNextSnapshotId(),
                CustomerId = customer.Id,
                CustomerName = orderCreateModel.CustomerName,
                CustomerEmail = orderCreateModel.CustomerEmail,
                ShippingAddress = orderCreateModel.ShippingAddress,
                CustomerPhoneNumber = orderCreateModel.CustomerPhoneNumber,
                Note = orderCreateModel.Note,

                TotalPrice = 0,
                ShippingCharge = 0,
                FinalAmount = 0,
                DiscountAmount = 0,

                Items = new List<PaymentSnapshotItem>(),

                CreatedAt = TimeZoneHelper.GetUtcNow(),
                UpdatedAt = TimeZoneHelper.GetUtcNow(),
                CreatedBy = customer.Id,
                EntityStatus = EEntityStatus.Active,
            };

            foreach (var item in orderCreateModel.Items)
            {
                var pVO = await _uow.ProductVariantOptions.GetOrderItemDetailAsync(item.ProductVariantOptionId);

                if (pVO == null)
                {
                    return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.NotFound, Messenger.NoExitData + " " + item.ProductVariantOptionId);
                }

                if (pVO.Stock < item.Quantity)
                {
                    return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.BadRequest, Messenger.NoExitData + " " + item.ProductVariantOptionId);
                }
                else
                {
                    snapshot.Items.Add(new PaymentSnapshotItem
                    {
                        Id = Guid.NewGuid(),
                        PublicId = ShareFunctions.GenarateRandomStringId(),
                        PaymentSnapshotId = snapshot.Id,
                        PaymentSnapshot = snapshot,
                        ProductVariantOptionId = pVO.Id,
                        ProductVariantOptionPublicId = pVO.PublicId,

                        CategoryName = pVO.ProductVariant.Product.Category.Name,
                        ProductName = pVO.ProductVariant.Product.Name + " " + pVO.ProductVariant.Name + " " + pVO.Name,
                        UrlImage = pVO.ImageUrl,

                        Quantity = item.Quantity,
                        PriceAtOrderTime = pVO.Price,
                        TotalPrice = item.Quantity * pVO.Price,

                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        UpdatedAt = TimeZoneHelper.GetUtcNow(),
                        CreatedBy = customer.Id,
                        EntityStatus = EEntityStatus.Active,
                    });
                }

                decimal itemTotal = item.Quantity * pVO.Price;
                totalPrice += itemTotal;
            }

            // Calculate Discount
            if (voucher != null)
            {
                if (voucher.DiscountType == EDiscountType.Percentage)
                {
                    discountedAmount = voucher.DiscountValue * totalPrice;

                    if (discountedAmount > voucher.MaxDiscountAmount) discountedAmount = voucher.MaxDiscountAmount;
                }
                else
                {
                    discountedAmount = voucher.DiscountValue;
                }

                snapshot.VoucherId = voucher.Id;

                voucher.ReservedCount++;
                _uow.Vouchers.Update(voucher);
            }

            var finalAmount = totalPrice - discountedAmount;

            if (discountedAmount > totalPrice)
            {
                finalAmount = 0;
            }

            snapshot.TotalPrice = totalPrice;
            snapshot.DiscountAmount = discountedAmount;
            snapshot.ShippingCharge = shippingCharge;
            snapshot.FinalAmount = totalPrice - discountedAmount + shippingCharge;


            var paymentQrUrl = await _vietQrService.GenerateQrAsync(snapshot.FinalAmount, snapshot.PublicId);

            if (paymentQrUrl == null)
            {
                return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.SystemError, Messenger.SystemError);
            }

            var paymentData = new PaymentDataForSnapshotModel()
            {
                SnapshotId = snapshot.PublicId,
                QrDataURL = paymentQrUrl,
                Amount = snapshot.FinalAmount,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                ExpiredAt = TimeZoneHelper.GetUtcNow().AddMinutes(15),
            };

            var idempotencyKeyEntry = new IdempotencyKey
            {
                Id = Guid.NewGuid(),
                PublicId = ShareFunctions.GenarateRandomStringId(),
                UserId = customer.Id,
                Endpoint = "/api/payments/snapshot",
                StatusCode = 200,
                ExpiredAt = TimeZoneHelper.GetUtcNow().AddHours(24),
                RequestKey = idempotencyKey,
                RequestHash = ShareFunctions.ComputeHash(orderCreateModel),
                ResponseBody = JsonSerializer.Serialize(paymentData),
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                EntityStatus = EEntityStatus.Active,
            };

            await _uow.PaymentSnapshots.AddAsync(snapshot);

            await _uow.IdempotencyKeys.AddAsync(idempotencyKeyEntry);

            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.SystemError, Messenger.SystemError);
            }

            return ServiceResult<PaymentDataForSnapshotModel>.Success(paymentData);
        }

        public async Task<ServiceResult<PaymentDataModel>> CreatePaymentForInvoiceByAdminAsync(string userId, PaymentCreateModel model)
        {
            var serviceResult = new ServiceResult<PaymentDataModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.NoExitData,
            };

            var cashier = await _uow.Users.GetByIdAsync(userId);
            if (cashier == null)
            {
                return serviceResult;
            }
            if (cashier.RoleId != ERole.Admin)
            {
                serviceResult.Message = Messenger.NoPermission;
                return serviceResult;
            }


            var invoice = await _uow.Invoices.TableNoTracking
                .Where(i => i.PublicId == model.InvoiceId)
                .Include(i => i.Order)
                .FirstOrDefaultAsync();

            if (invoice == null) return serviceResult;

            if(invoice.RemainingAmount <= 0)
            {
                serviceResult.Message = PaymentMessenger.PaymentIsPaid;
                return serviceResult;
            }

            var paymentId = await _sequenceService.GetNextPaymentIdAsync();

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                PublicId = paymentId,
                InvoiceId = invoice.Id,
                UserId = invoice.Order.CustomerId,
                PaymentMethod = EPaymentMethod.DomesticBank,
                PaymentStatus = EPaymentStatus.Pending,
                PaymentCode = "",
                BankReferenceCode = "",
                TransactionId = "",
                Amount = model.Amount,
                CreatedBy = cashier.Id,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                EntityStatus = EEntityStatus.Active
            };
            

            await _uow.Payments.AddAsync(payment);

            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            var paymentQrUrl = await _vietQrService.GenerateQrAsync(payment.Amount, payment.PublicId);

            if (paymentQrUrl == null)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.Data = new PaymentDataModel()
            {
                PaymentId = payment.PublicId,
                QrDataURL = paymentQrUrl,
                Amount = payment.Amount,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                ExpiredAt = TimeZoneHelper.GetUtcNow().AddMinutes(15),
            };

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<VerifyResult>> VerifyPaymentForInvoiceAsync(PaymentForInvocieWebhookRequest request)
        {
            var serviceResult = new ServiceResult<VerifyResult>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.NoExitData,
            };

            var payment = await _uow.Payments.Table
                .Where(p => p.PublicId == request.PaymentId)
                .Include(p => p.Invoice)
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return serviceResult;
            }

            payment.PaymentStatus = EPaymentStatus.Paid;

            var invoice = payment.Invoice;

            if (payment == null)
            {
                return serviceResult;
            }

            //if (invoice.RemainingAmount <= 0)
            //{
            //    serviceResult.Message = PaymentMessenger.PaymentIsPaid;
            //    return serviceResult;
            //}

            invoice.PaidAmount += payment.Amount;

            if (invoice.RemainingAmount == 0)
            {
                invoice.InvoiceStatus = EInvoiceStatus.Paid;
                invoice.PaidAt = TimeZoneHelper.GetUtcNow();
            }

            _uow.Payments.Update(payment);
            _uow.Invoices.Update(invoice);
            var result = await _uow.CommitAsync();

            if(result < 1)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.Data = new VerifyResult()
            {
                SnapshotId = payment.PublicId,
                Amount = payment.Amount,
                Message = PaymentMessenger.PaymentVerified
            };

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }
    }
}
