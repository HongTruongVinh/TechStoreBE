using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
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
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;
        private readonly IQRCodeService _qrCodeService;
        private readonly IVietQrService _vietQrService;

        public OrderService(IUnitOfWork uow,
            SequenceGeneratorService sequenceService,
            IQRCodeService qRCodeService,
            IVietQrService vietQrService
            )
        {
            _uow = uow;
            _sequenceService = sequenceService;
            _qrCodeService = qRCodeService;
            _vietQrService = vietQrService;
        }

        public async Task<ServiceResult<string>> CreatePrePayOnlineOrderAsync(string userId, PaymentSnapshot ps, PaymentForSnapshotWebhookRequest paymentData)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.BadRequest,
            };

            var customer = await _uow.Users.GetByIdAsync(userId);
            if (customer == null)
            {
                serviceResult.Message = Messenger.NoExitData + " " + userId;
                return serviceResult;
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                PublicId = await _sequenceService.GetNextOrderIdAsync(),
                Customer = customer,
                CustomerId = customer.Id,
                CustomerPublicId = customer.PublicId,

                CustomerName = ps.CustomerName,
                CustomerPhoneNumber = ps.CustomerPhoneNumber,
                CustomerEmail = ps.CustomerEmail,
                ShippingAddress = ps.ShippingAddress,
                Note = ps.Note,

                TotalPrice = ps.TotalPrice,
                ShippingCharge = ps.ShippingCharge,
                DiscountAmount = ps.DiscountAmount,
                FinalAmount = ps.FinalAmount,

                OrderStatus = EOrderStatus.Processing,
                OrderItems = new List<OrderItem>(),

                CreatedAt = TimeZoneHelper.GetUtcNow(),
                UpdatedAt = TimeZoneHelper.GetUtcNow(),
                CreatedBy = customer.Id,
                EntityStatus = EEntityStatus.Active,
            };

            foreach (var item in ps.Items)
            {
                var pvo = await _uow.ProductVariantOptions.FindOneAsync(pvo => pvo.Id == item.ProductVariantOptionId);

                if (pvo != null)
                {
                    pvo.SoldCount += item.Quantity;
                    pvo.Stock -= item.Quantity;
                    _uow.ProductVariantOptions.Update(pvo);

                    order.OrderItems.Add(new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        PublicId = $"{DateTime.UtcNow:yyyyMMdd}{(Random.Shared.Next(100000, 1000000).ToString() + 1):D6}",
                        ProductVariantOptionId = item.ProductVariantOptionId,
                        ProductVariantOptionPublicId = item.ProductVariantOptionPublicId,
                        Order = order,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        ImageUrl = item.UrlImage,
                        CategoryName = item.CategoryName,
                        ProductName = item.ProductName,
                        PriceAtOrderTime = item.PriceAtOrderTime,
                        TotalPrice = item.TotalPrice,

                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        EntityStatus = EEntityStatus.Active,
                    });
                }
            }

            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                PublicId = await _sequenceService.GetNextInvoiceIdAsync(),

                TotalAmount = ps.FinalAmount,
                PaidAmount = paymentData.Amount,

                Payments = new List<Payment>(),

                CreatedAt = TimeZoneHelper.GetUtcNow(),
                CreatedBy = customer.Id,
                InvoiceStatus = paymentData.Amount >= ps.FinalAmount ? EInvoiceStatus.Paid : EInvoiceStatus.PartiallyPaid,
                EntityStatus = EEntityStatus.Active
            };

            Payment payment = new Payment
            {
                Id = Guid.NewGuid(),
                PublicId = await _sequenceService.GetNextPaymentIdAsync(),
                Invoice = invoice,
                InvoiceId = invoice.Id,
                User = customer,
                UserId = customer.Id,
                Amount = paymentData.Amount,
                PaymentMethod = EPaymentMethod.DomesticBank,
                TransactionCode = paymentData.TransactionId,
                PaymentStatus = EPaymentStatus.Paid,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                EntityStatus = EEntityStatus.Active,
            };

            invoice.Payments.Add(payment);
            order.Invoice = invoice;

            await _uow.Orders.AddAsync(order);

            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = order.PublicId;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<string>> CreateCODOnlineOrderAsync(string userId, OrderCreateModel orderCreateModel)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.BadRequest,
            };

            var customer = await _uow.Users.GetByIdAsync(userId);
            if (customer == null)
            {
                serviceResult.Message = Messenger.NoExitData + " " + userId;
                return serviceResult;
            }

            var orderId = await _sequenceService.GetNextOrderIdAsync();

            decimal totalPrice = 0;
            decimal totalDiscount = 0;
            var order = new Order
            {
                Id = Guid.NewGuid(),
                PublicId = orderId,
                Customer = customer,
                CustomerId = customer.Id,
                CustomerPublicId = customer.PublicId,
                CustomerName = orderCreateModel.CustomerName,
                CustomerPhoneNumber = orderCreateModel.CustomerPhoneNumber,
                CustomerEmail = orderCreateModel.CustomerEmail,
                TotalPrice = 0,
                ShippingCharge = 0,
                DiscountAmount = totalDiscount,
                FinalAmount = 0,
                OrderStatus = EOrderStatus.Pending,
                OrderItems = new List<OrderItem>(),
                ShippingAddress = orderCreateModel.ShippingAddress,
                Note = orderCreateModel.Note,

                CreatedAt = TimeZoneHelper.GetUtcNow(),
                UpdatedAt = TimeZoneHelper.GetUtcNow(),
                CreatedBy = customer.Id,
                EntityStatus = EEntityStatus.Active,
            };


            var today = DateTime.UtcNow.Date;
            foreach (var item in orderCreateModel.Items)
            {
                var pVO = await _uow.ProductVariantOptions.GetOrderItemDetailAsync(item.ProductVariantOptionId);

                if (pVO == null)
                {
                    serviceResult.Message = Messenger.NoExitData + " " + item.ProductVariantOptionId;
                    return serviceResult;
                }

                if (pVO.Stock < item.Quantity)
                {
                    serviceResult.Message = Messenger.NoExitData + " " + item.ProductVariantOptionId;
                    return serviceResult;
                }
                else
                {
                    pVO.Stock -= item.Quantity;
                    _uow.ProductVariantOptions.Update(pVO);

                    order.OrderItems.Add(new OrderItem
                    {
                        PublicId = $"{today:yyyyMMdd}{(Random.Shared.Next(100000, 1000000).ToString() + 1):D6}",
                        ProductVariantOptionId = pVO.Id,
                        ProductVariantOptionPublicId = pVO.PublicId,
                        OrderId = order.Id,
                        Order = order,
                        CategoryName = pVO.ProductVariant.Product.Category.Name,
                        ProductName = pVO.ProductVariant.Product.Name + " " + pVO.ProductVariant.Name + " " + pVO.Name,
                        ImageUrl = pVO.ImageUrl,
                        Quantity = item.Quantity,
                        PriceAtOrderTime = pVO.Price,
                        TotalPrice = item.Quantity * pVO.Price,
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        EntityStatus = EEntityStatus.Active,
                    });
                }

                decimal itemTotal = pVO.Price * item.Quantity;
                totalPrice += itemTotal;
            }

            var finalAmount = totalPrice - totalDiscount;

            order.TotalPrice = totalPrice;
            order.DiscountAmount = totalDiscount;
            order.FinalAmount = finalAmount;

            await _uow.Orders.AddAsync(order);

            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = order.PublicId;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
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
                TotalAmount = order.FinalAmount,
                PaidAmount = 0,
                Payments = new List<Payment>(),
                InvoiceStatus = EInvoiceStatus.Unpaid,
                EntityStatus = EEntityStatus.Active,
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
                EntityStatus = EEntityStatus.Active,
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

            var order = await _uow.Orders.GetByIdAsync(orderId);
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
