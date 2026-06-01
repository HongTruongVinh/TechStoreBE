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
using TechStore.Model.DTOs.RequestModel;
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

            if (orderCreateModel.PaymentMethod != EPaymentMethod.COD)
            {
                serviceResult.Message = Messenger.NoExitData + " Invalid Payment Method";
                return serviceResult;
            }

            var orderId = await _sequenceService.GetNextOrderIdAsync();

            //orderCreateModel.CustomerId = userId;

            // Tính toán tổng tiền
            decimal totalPrice = 0;
            decimal totalDiscount = 0;
            var order = new Order
            {
                Id = Guid.NewGuid(),
                PublicId = orderId,
                Customer = customer,
                CustomerId = customer.Id,
                OrderType = EOrderType.Online,
                CustomerName = orderCreateModel.CustomerName,
                CustomerPhoneNumber = orderCreateModel.CustomerPhoneNumber,
                CustomerEmail = orderCreateModel.CustomerEmail ?? "",
                TotalPrice = 0,
                ShippingCharge = 0,
                DiscountAmount = totalDiscount,
                FinalAmount = 0,
                PaymentMethod = EPaymentMethod.COD,
                Payments = new List<Payment>(),
                OrderStatus = EOrderStatus.Pending,
                OrderItems = new List<OrderItem>(),
                ShippingAddress = orderCreateModel.ShippingAddress,
                Note = orderCreateModel.Note,

                CreatedAt = TimeZoneHelper.GetUtcNow(),
                UpdatedAt = TimeZoneHelper.GetUtcNow(),
                CreatedBy = customer.Id,
                EntityStatus = EEntityStatus.Active,
            };

            //await _uow.Orders.AddAsync(order);
            //await _uow.CommitAsync();

            var today = DateTime.UtcNow.Date;
            foreach (var item in orderCreateModel.Items)
            {
                var productVariantOption = await _uow.ProductVariantOptions.GetProductVariantOptionDetailByPublicIdAsync(item.ProductVariantOptionId);

                if (productVariantOption == null)
                {
                    serviceResult.Message = Messenger.NoExitData + " " + item.ProductVariantOptionId;
                    return serviceResult;
                }

                if (productVariantOption.Stock < item.Quantity)
                {
                    serviceResult.Message = Messenger.NoExitData + " " + item.ProductVariantOptionId;
                    return serviceResult;
                }
                else
                {
                    productVariantOption.Stock -= item.Quantity;
                    _uow.ProductVariantOptions.Update(productVariantOption);

                    order.OrderItems.Add(new OrderItem
                    {
                        PublicId = $"{today:yyyyMMdd}{(Random.Shared.Next(100000, 1000000).ToString() + 1):D6}",
                        ProductVariantOptionId = productVariantOption.Id,
                        ProductVariantOptionPublicId = productVariantOption.PublicId,
                        OrderId = order.Id,
                        Order = order,
                        Quantity = item.Quantity,
                        PriceAtOrderTime = productVariantOption.ProductVariant.Price,
                        Discount = item.Discount,
                        TotalPrice = item.Quantity * productVariantOption.ProductVariant.Price,
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        EntityStatus = EEntityStatus.Active,
                    });
                }

                decimal itemTotal = productVariantOption.ProductVariant.Price * item.Quantity;
                totalPrice += itemTotal;
                totalDiscount += item.Discount;
            }

            var finalAmount = totalPrice - totalDiscount;

            //await _uow.OrderItems.AddRangeAsync(orderItems);


            //string orderTrackingQR = ServerAddress.WEBSITE_ADDRESS;
            //var addQRCodeResult = await _qrCodeService.AddTrackingOrderQRCodeAsync(order, orderTrackingQR, order.PublicId, EQRCodeType.OrderTracking, null);

            //if (addQRCodeResult.IsSuccess == false)
            //{
            //    serviceResult.Message = Messenger.SystemError;
            //    return serviceResult;
            //}

            order.TotalPrice = totalPrice;
            order.DiscountAmount = totalDiscount;
            order.FinalAmount = finalAmount;
            //order.OrderItems = orderItems;

            await _uow.Orders.AddAsync(order);
            //_uow.Orders.Update(order);

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

        public async Task<ServiceResult<string>> CreatePrePayOnlineOrderAsync(string userId, string paymentId, OrderCreateModel orderCreateModel)
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

            if (orderCreateModel.PaymentMethod != EPaymentMethod.DomesticBank)
            {
                serviceResult.Message = Messenger.NoExitData + " Invalid Payment Method";
                return serviceResult;
            }

            var payment = await _uow.Payments.FindOneAsync(p => p.PublicId == paymentId);
            if (payment == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var orderId = await _sequenceService.GetNextOrderIdAsync();
            var invoiceId = await _sequenceService.GetNextInvoiceIdAsync();
            //var paymentId = await _sequenceService.GetNextPaymentIdAsync();

            // Tính toán tổng tiền
            decimal totalPrice = 0;
            decimal totalDiscount = 0;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                PublicId = orderId,
                Customer = customer,
                CustomerId = customer.Id,
                OrderType = EOrderType.Online,
                CustomerName = orderCreateModel.CustomerName,
                CustomerPhoneNumber = orderCreateModel.CustomerPhoneNumber,
                CustomerEmail = orderCreateModel.CustomerEmail ?? "",
                TotalPrice = 0,
                ShippingCharge = 0,
                DiscountAmount = totalDiscount,
                FinalAmount = 0,
                PaymentMethod = EPaymentMethod.DomesticBank,
                OrderStatus = EOrderStatus.Processing,
                OrderItems = new List<OrderItem>(),
                ShippingAddress = orderCreateModel.ShippingAddress,
                Note = orderCreateModel.Note,
                Payments = new List<Payment>(),

                CreatedAt = TimeZoneHelper.GetUtcNow(),
                UpdatedAt = TimeZoneHelper.GetUtcNow(),
                CreatedBy = customer.Id,
                EntityStatus = EEntityStatus.Active,
            };

            //await _uow.Orders.AddAsync(order);
            //await _uow.CommitAsync();
            var today = DateTime.UtcNow.Date;
            foreach (var item in orderCreateModel.Items)
            {
                var productVariantOption = await _uow.ProductVariantOptions.GetProductVariantOptionDetailByPublicIdAsync(item.ProductVariantOptionId);

                if (productVariantOption == null)
                {
                    serviceResult.Message = Messenger.NoExitData + " " + item.ProductVariantOptionId;
                    return serviceResult;
                }

                if (productVariantOption.Stock < item.Quantity)
                {
                    serviceResult.Message = Messenger.NoExitData + " " + item.ProductVariantOptionId;
                    return serviceResult;
                }
                else
                {
                    productVariantOption.Stock -= item.Quantity;
                    _uow.ProductVariantOptions.Update(productVariantOption);

                    order.OrderItems.Add(new OrderItem
                    {
                        PublicId = $"{today:yyyyMMdd}{(Random.Shared.Next(100000, 1000000).ToString() + 1):D6}",
                        ProductVariantOptionId = productVariantOption.Id,
                        ProductVariantOptionPublicId = productVariantOption.PublicId,
                        Order = order,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        PriceAtOrderTime = productVariantOption.ProductVariant.Price,
                        Discount = item.Discount,
                        TotalPrice = item.Quantity * productVariantOption.ProductVariant.Price,
                        CreatedAt = TimeZoneHelper.GetUtcNow(),
                        EntityStatus = EEntityStatus.Active,
                    });
                }

                decimal itemTotal = item.Quantity * productVariantOption.ProductVariant.Price;
                totalPrice += itemTotal;
                totalDiscount += item.Discount;
            }

            var finalAmount = totalPrice - totalDiscount;

            var invoice = new Invoice
            {
                PublicId = invoiceId,
                OrderId = order.Id,
                Order = order,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                CreatedBy = customer.Id,
                TotalPrice = totalPrice,
                DiscountAmount = 0,
                FinalAmount = finalAmount,
                InvoiceStatus = EInvoiceStatus.Unpaid,
                EntityStatus = EEntityStatus.Active,
            };


            //string orderTrackingQR = ServerAddress.WEBSITE_ADDRESS;
            //var addQRCodeResult = await _qrCodeService.AddTrackingOrderQRCodeAsync(order, orderTrackingQR, order.PublicId, EQRCodeType.OrderTracking, null);

            //if (addQRCodeResult.IsSuccess == false)
            //{
            //    serviceResult.Message = Messenger.SystemError;
            //    return serviceResult;
            //}

            order.Invoice = invoice;
            order.Payments.Add(payment);
            order.TotalPrice = totalPrice;
            order.DiscountAmount = totalDiscount;
            order.FinalAmount = finalAmount;

            //await _uow.OrderItems.AddRangeAsync(orderItems);
            await _uow.Orders.AddAsync(order);
            //await _uow.Invoices.AddAsync(invoice);

            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            //var paymentQrUrl = await _vietQrService.GenerateQrAsync(payment.Amount, $"PAY {order.PublicId}");

            //if (paymentQrUrl == null)
            //{
            //    serviceResult.Message = Messenger.SystemError;
            //    return serviceResult;
            //}

            serviceResult.Data = order.PublicId;

            serviceResult.IsSuccess = true;
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

            var orderItems = order.OrderItems;
            if (orderItems != null && orderItems.Count > 0)
            {
                _uow.OrderItems.RemoveRange(orderItems);
            }

            if (order.Invoice != null)
            {
                var invoice = await _uow.Invoices.FindOneAsync(i => i.Id == order.Invoice.Id);
                if (invoice != null)
                {
                    _uow.Invoices.Remove(invoice);
                }
            }

            foreach (var payment in order.Payments)
            {
                var existPayment = await _uow.Payments.FindOneAsync(p => p.Id == payment.Id);
                if (existPayment != null)
                {
                    var qrPayments = await _uow.QRCodes.FindOneAsync(q => q.RelatedId == existPayment.Id);

                    if (qrPayments != null)
                    {
                        _uow.QRCodes.Remove(qrPayments);
                    }

                    _uow.Payments.Remove(existPayment);
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


        public async Task<ServiceResult<List<OrderDetailResponseModel>>> GetListOrdersByStatusIdAsync(EOrderStatus statusId)
        {
            var serviceResult = new ServiceResult<List<OrderDetailResponseModel>>
            {
                IsSuccess = true,
                Data = new List<OrderDetailResponseModel>(),
                Message = Messenger.GetDataSuccessful,
            };

            var listOrders = await _uow.Orders.GetOrdersIncludeItemsDetailAsync(o => o.OrderStatus == statusId);

            foreach (var order in listOrders)
            {
                var customer = await _uow.Users.GetByInternalIdAsync(order.CustomerId);

                serviceResult.Data.Add(order.ToOrderDetailModel(customer));
            }

            return serviceResult;
        }

        public async Task<ServiceResult<List<OrderResponseModel>>> GetAllOrdersAsync()
        {
            var serviceResult = new ServiceResult<List<OrderResponseModel>>
            {
                IsSuccess = true,
                Data = new List<OrderResponseModel>(),
                Message = Messenger.GetDataSuccessful,
            };

            var listOrders = await _uow.Orders.GetAllAsync();

            foreach (var order in listOrders)
            {
                var customer = await _uow.Users.GetByInternalIdAsync(order.CustomerId);

                if (customer != null)
                {
                    serviceResult.Data.Add(order.ToOrderResponseModel(customer));
                }

                //serviceResult.Data.Add(await OrderToOrderModelAsync(order));
            }

            return serviceResult;
        }

        public async Task<ServiceResult<List<OrderListItemModel>>> GetOnlineOrdersAsync()
        {
            var serviceResult = new ServiceResult<List<OrderListItemModel>>
            {
                IsSuccess = true,
                Data = new List<OrderListItemModel>(),
                Message = Messenger.GetDataSuccessful,
            };

            var listOrders = await _uow.Orders.FindManyAsync(o => o.OrderType == EOrderType.Online);

            foreach (var order in listOrders)
            {
                serviceResult.Data.Add(order.ToOrderListItemModel());
            }

            return serviceResult;
        }

        public async Task<ServiceResult<List<OrderListItemModel>>> GetInStoreOrdersAsync()
        {
            var serviceResult = new ServiceResult<List<OrderListItemModel>>
            {
                IsSuccess = true,
                Data = new List<OrderListItemModel>(),
                Message = Messenger.GetDataSuccessful,
            };

            var listOrders = await _uow.Orders.FindManyAsync(o => o.OrderType == EOrderType.InStore);

            foreach (var order in listOrders)
            {
                serviceResult.Data.Add(order.ToOrderListItemModel());
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

            var order = await _uow.Orders.GetOrderIncludeItemsAsync(p => p.PublicId == orderId);

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

            order.QRCode = await _uow.QRCodes.FindOneAsync(q => q.RelatedId == order.Id && q.Type == EQRCodeType.OrderTracking);
            order.Invoice = await _uow.Invoices.FindOneAsync(i => i.OrderId == order.Id);
            var payments = await _uow.Payments.FindManyAsync(p => p.OrderId == order.Id);
            foreach ( var payment in payments)
            {
                order.Payments.Add(payment);
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

            serviceResult.Data = order.ToOrderDetailModel(customer);
            //serviceResult.Data = await OrderToOrderDetailModelAsync(order);
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
            if (adminUser != null) {
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

            var order = await _uow.Orders.GetOrderIncludeItemsAsync(p => p.PublicId == orderId);

            if (order == null)
            {
                return serviceResult;
            }

            var customer = await _uow.Users.GetByInternalIdAsync(order.CustomerId);
            if (customer == null)
            {
                serviceResult.Message = Messenger.NoExitData + " " + userId;
                return serviceResult;
            }

            if (customer.Id != order.CustomerId)
            {
                return serviceResult;
            }

            order.QRCode = await _uow.QRCodes.FindOneAsync(q => q.RelatedId == order.Id && q.Type == EQRCodeType.OrderTracking);
            order.Invoice = await _uow.Invoices.FindOneAsync(i => i.OrderId == order.Id);
            var payments = await _uow.Payments.FindManyAsync(p => p.OrderId == order.Id);
            foreach (var payment in payments)
            {
                order.Payments.Add(payment);
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

            serviceResult.Data = order.ToOrderDetailModel(customer);
            //serviceResult.Data = await OrderToOrderDetailModelAsync(order);
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

            order.OrderStatus = EOrderStatus.Processing;
            order.UpdatedAt = TimeZoneHelper.GetUtcNow();

            order.UpdatedBy = userUpdating.Id;

            decimal totalPrice = 0;
            var listOrderItems = await _uow.OrderItems.FindManyAsync(oi => oi.OrderId == order.Id);

            foreach (var item in listOrderItems)
            {
                totalPrice += item.TotalPrice;
            }

            var invoice = await _uow.Invoices.FindOneAsync(i => i.OrderId == order.Id);
            if (invoice == null)
            {
                var invoiceId = await _sequenceService.GetNextInvoiceIdAsync();
                var newInvoice = new Invoice
                {
                    PublicId = invoiceId,
                    OrderId = order.Id,
                    Order = order,
                    CreatedAt = TimeZoneHelper.GetUtcNow(),
                    CreatedBy = userUpdating.Id,
                    TotalPrice = totalPrice,
                    DiscountAmount = 0,
                    FinalAmount = order.FinalAmount,
                    InvoiceStatus = EInvoiceStatus.Unpaid,
                    EntityStatus = EEntityStatus.Active,
                };

                await _uow.Invoices.AddAsync(newInvoice);

                order.Invoice = newInvoice;
            }

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
                ShipperId = shipper.Id,
                Shipper = shipper,
                ShippedDate = DateTime.UtcNow,
                TrackingNumber = Random.Shared.Next(1001, 10000).ToString(),
                Status = EShippingDetailStatus.Shipping,
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
                invoice.InvoiceStatus = EInvoiceStatus.Paid;
                _uow.Invoices.Update(invoice);

                var payment = await _uow.Payments.FindOneAsync(p => p.OrderId == order.Id);
                if (payment != null)
                {
                    payment.PaymentStatus = EPaymentStatus.Paid;
                    payment.UpdatedAt = TimeZoneHelper.GetUtcNow();
                    _uow.Payments.Update(payment);
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

        public async Task<ServiceResult<bool>> UpdateOrderStatusToCanceledAsync(string updateByUserId, string orderId, CancelOrderModel orderUpdateStatusModel)
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

            order.OrderStatus = EOrderStatus.Canceled;
            order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            order.Note += " - " + TimeZoneHelper.GetUtcNow().ToString() + ": Reason cancel: " + orderUpdateStatusModel.Reason;
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

        //private async Task<OrderResponseModel> OrderToOrderModelAsync(Order order)
        //{
        //    var orderResponseModel = new OrderResponseModel
        //    {
        //        OrderId = order.PublicId,
        //        CustomerId = order.CustomerId,
        //        CustomerName = order.CustomerName ?? "",
        //        ShippingAddress = order.ShippingAddress ?? "",
        //        CustomerPhonenumber = order.CustomerPhoneNumber ?? "",
        //        CustomerEmail = order.CustomerEmail ?? "",
        //        TotalPrice = order.TotalPrice,
        //        DiscountAmount = order.DiscountAmount,
        //        ShippingCharge = order.ShippingCharge,
        //        FinalAmount = order.FinalAmount,
        //        PaymentMethod = order.PaymentMethod,
        //        OrderType = order.OrderType,
        //        OrderStatus = order.OrderStatus,
        //        CreatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.CreatedAt ?? TimeZoneHelper.GetGmt7Now()),
        //        UpdatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.UpdatedAt ?? TimeZoneHelper.GetGmt7Now()),
        //        Items = new List<OrderItemResponseModel>()
        //    };

        //    var orderItems = await _orderItemRepository.GetOrderItemsByOrderIdAsync(order.OrderId);

        //    foreach (var orderItem in orderItems)
        //    {
        //        var product = await _productRepository.GetByIdAsync(orderItem.ProductId);
        //        var category = await _categoryRepository.GetByIdAsync(product.CategoryId);

        //        var orderItemResponeModel = new OrderItemResponseModel
        //        {
        //            ProductId = orderItem.ProductId,
        //            OrderId = orderItem.OrderId,
        //            Quantity = orderItem.Quantity,
        //            PriceAtOrderTime = orderItem.PriceAtOrderTime,
        //            Discount = orderItem.Discount,
        //            TotalPrice = orderItem.TotalPrice,
        //            ProductName = product.Name,
        //            CategoryName = category.Name,
        //            MainImageUrl = product.MainImageUrl,
        //        };

        //        orderResponseModel.Items.Add(orderItemResponeModel);
        //    }

        //    if (order.ShippingDetailId != null)
        //    {
        //        var shippingDetail = await _shippingDetailRepository.GetByIdAsync(order.ShippingDetailId);
        //        if (shippingDetail != null)
        //        {
        //            var shipper = await _shipperRepository.GetByIdAsync(shippingDetail.ShipperId);

        //            orderResponseModel.ShippingDetailId = shipper.ShipperId;
        //            orderResponseModel.ShipperName = shipper.Name;
        //            orderResponseModel.TrackingNumber = shippingDetail.TrackingNumber;
        //            orderResponseModel.ShippedDate = shippingDetail.ShippedDate;
        //            orderResponseModel.EstimatedArrival = shippingDetail.EstimatedArrival;
        //            orderResponseModel.ShippingNote = shippingDetail.ShippingNote;
        //            orderResponseModel.FailureCount = shippingDetail.FailureCount;
        //        }
        //    }

        //    return orderResponseModel;
        //}

        //private async Task<OrderDetailResponseModel> OrderToOrderDetailModelAsync(Order order)
        //{
        //    OrderDetailResponseModel orderDetailResponseModel = new OrderDetailResponseModel
        //    {
        //        OrderId = order.OrderId,
        //        CustomerId = order.CustomerId ?? "",
        //        CustomerName = order.CustomerName,
        //        ShippingAddress = order.ShippingAddress ?? "",
        //        CustomerPhonenumber = order.CustomerPhoneNumber,
        //        CustomerEmail = order.CustomerEmail ?? "",
        //        TotalPrice = order.TotalPrice,
        //        ShippingCharge = order.ShippingCharge,
        //        DiscountAmount = order.DiscountAmount,
        //        FinalAmount = order.FinalAmount,
        //        PaymentMethod = order.PaymentMethod,
        //        OrderStatus = order.OrderStatus,
        //        CreatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.CreatedAt ?? TimeZoneHelper.GetGmt7Now()),
        //        UpdatedAt = TimeZoneHelper.ConvertUtcToGmt7(order.UpdatedAt ?? TimeZoneHelper.GetGmt7Now()),
        //        Items = new List<OrderItemResponseModel>(),

        //    };

        //    var orderItems = await _orderItemRepository.GetOrderItemsByOrderIdAsync(order.OrderId);

        //    foreach (var item in orderItems)
        //    {

        //        var product = await _productRepository.GetByIdAsync(item.ProductId);
        //        var category = await _categoryRepository.GetByIdAsync(product.CategoryId);

        //        orderDetailResponseModel.Items.Add(new OrderItemResponseModel
        //        {
        //            OrderId = item.OrderId,
        //            ProductId = item.ProductId,
        //            ProductName = product.Name,
        //            CategoryName = category.Name,
        //            Quantity = item.Quantity,
        //            PriceAtOrderTime = item.PriceAtOrderTime,
        //            Discount = item.Discount,
        //            TotalPrice = item.TotalPrice,
        //            MainImageUrl = product.MainImageUrl
        //        });
        //    }


        //    if (order.QRCodeId != null)
        //    {
        //        var qrCode = await _qrCodeService.GetQRCodeAsync(order.OrderId, EQRCodeType.OrderTracking);
        //        if (qrCode.Data != null)
        //        {

        //            orderDetailResponseModel.QRCode = Convert.ToBase64String(qrCode.Data.ImageData);
        //        }
        //    }

        //    if (order.InvoiceId != null)
        //    {
        //        var invoice = await _invoiceRepository.GetInvoiceByOrderIdAsync(order.OrderId);
        //        if (invoice != null)
        //        {
        //            orderDetailResponseModel.Invoice = invoice.ToInvoiceModel(order);
        //        }

        //        if (order.PaymentId != null)
        //        {
        //            var payment = await _paymentRepository.GetPaymentByOrderIdAsync(order.OrderId);

        //            if (payment != null)
        //            {
        //                orderDetailResponseModel.Payment = new PaymentResponseModel
        //                {
        //                    OrderId = payment.OrderId,
        //                    PaymentId = payment.PaymentId,
        //                    CustomerId = order.CustomerId ?? "",
        //                    CustomerName = order.CustomerName,
        //                    CustomerPhonenumber = order.CustomerPhoneNumber,
        //                    PaymentMethod = payment.PaymentMethod,
        //                    Amount = payment.Amount,
        //                    TransactionCode = payment.TransactionCode,
        //                    PaymentStatus = payment.PaymentStatus,
        //                    CreatedAt = payment.CreatedAt,
        //                };
        //            }
        //        }

        //        if (order.ShippingDetailId != null)
        //        {
        //            var shippingDetail = await _shippingDetailRepository.GetByIdAsync(order.ShippingDetailId);
        //            if (shippingDetail != null)
        //            {
        //                var shipper = await _shipperRepository.GetByIdAsync(shippingDetail.ShipperId);

        //                orderDetailResponseModel.ShippingDetailId = shipper.ShipperId;
        //                orderDetailResponseModel.ShipperName = shipper.Name;
        //                orderDetailResponseModel.TrackingNumber = shippingDetail.TrackingNumber;
        //                orderDetailResponseModel.ShippedDate = shippingDetail.ShippedDate;
        //                orderDetailResponseModel.EstimatedArrival = shippingDetail.EstimatedArrival;
        //                orderDetailResponseModel.ShippingNote = shippingDetail.ShippingNote;
        //                orderDetailResponseModel.FailureCount = shippingDetail.FailureCount;
        //            }
        //        }
        //    }
        //    return orderDetailResponseModel;
        //}

        public async Task<ServiceResult<OrderDetailResponseModel>> SeedDataOrderAsync(string userId, OrderCreateModel orderCreateModel)
        {
            var serviceResult = new ServiceResult<OrderDetailResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.BadRequest,
            };

            //var orderId = await _sequenceService.GetNextOrderIdAsync();

            //// Tính toán tổng tiền
            //decimal totalPrice = 0;
            //decimal totalDiscount = 0;

            //foreach (var item in orderCreateModel.Items)
            //{
            //    item.OrderId = orderId;

            //    decimal itemTotal = item.PriceAtOrderTime * item.Quantity;
            //    totalPrice += itemTotal;
            //    totalDiscount += item.Discount;
            //    item.TotalPrice = itemTotal - item.Discount;
            //}

            //var finalAmount = totalPrice - totalDiscount;

            //string qrCodeContent = "orderId:" + orderId + ",userId:" + userId;
            ////var resultAddQRCode = await _qrCodeService.ServiceAddQRCodeAsync(qrCodeContent, orderId, EQRCodeType.OrderTracking, null);

            //var user = await _uow.Users.GetByIdAsync(orderCreateModel.CustomerId);
            //if (user == null)
            //{
            //    serviceResult.Message = Messenger.NoExitData + " UserId: " + orderCreateModel.CustomerId;
            //    return serviceResult;
            //}

            //var order = new Order
            //{
            //    PublicId = orderId,
            //    CustomerId = user.Id,
            //    OrderType = EOrderType.Online,
            //    CustomerName = orderCreateModel.CustomerName,
            //    ShippingAddress = orderCreateModel.ShippingAddress,
            //    CustomerPhoneNumber = orderCreateModel.CustomerPhonenumber,
            //    CustomerEmail = orderCreateModel.CustomerEmail,
            //    TotalPrice = totalPrice,
            //    ShippingCharge = 0, // Assuming no shipping charge for now
            //    DiscountAmount = totalDiscount,
            //    FinalAmount = finalAmount,
            //    PaymentMethod = orderCreateModel.PaymentMethod,
            //    //QRCodeId = resultAddQRCode.Id,
            //    OrderStatus = EOrderStatus.Pending,
            //    CreatedAt = new DateTime(2025, Random.Shared.Next(1, 6), Random.Shared.Next(1, 29)),
            //    UpdatedAt = TimeZoneHelper.GetUtcNow(),
            //    OrderItems = new List<OrderItem>(),
            //    CreatedBy = user.Id,
            //    EntityStatus = EEntityStatus.Active,
            //};

            //await _uow.Orders.AddAsync(order);

            //serviceResult.Data = new OrderDetailResponseModel
            //{
            //    OrderId = orderId,
            //    CustomerId = orderCreateModel.CustomerId,
            //    CustomerName = orderCreateModel.CustomerName,
            //    ShippingAddress = orderCreateModel.ShippingAddress,
            //    CustomerPhonenumber = orderCreateModel.CustomerPhonenumber,
            //    CustomerEmail = orderCreateModel.CustomerEmail,
            //    TotalPrice = totalPrice,
            //    ShippingCharge = order.ShippingCharge, // Assuming no shipping charge for now
            //    DiscountAmount = totalDiscount,
            //    FinalAmount = finalAmount,
            //    PaymentMethod = orderCreateModel.PaymentMethod,
            //    //QRCode = Convert.ToBase64String(resultAddQRCode.ImageData),
            //    OrderStatus = EOrderStatus.Pending,
            //    CreatedAt = TimeZoneHelper.GetUtcNow(),
            //    UpdatedAt = TimeZoneHelper.GetUtcNow(),
            //    Items = new List<OrderItemResponseModel>()
            //};

            //// Lưu các OrderItem
            //List<OrderItem> orderItems = new List<OrderItem>();
            //foreach (var item in orderCreateModel.Items)
            //{
            //    var product = await _uow.Products.GetByIdAsync(item.ProductVariantOptionId);
            //    if (product == null)
            //    {
            //        serviceResult.Message = Messenger.NoExitData + " ProductId: " + item.ProductVariantOptionId;
            //        return serviceResult;
            //    }

            //    orderItems.Add(new OrderItem
            //    {
            //        PublicId = Random.Shared.Next(100000, 1000000).ToString(),
            //        ProductId = product.Id,
            //        Product = product,
            //        Order = order,
            //        OrderId = order.Id,
            //        Quantity = item.Quantity,
            //        PriceAtOrderTime = item.PriceAtOrderTime,
            //        Discount = item.Discount,
            //        TotalPrice = item.TotalPrice,
            //        CreatedAt = TimeZoneHelper.GetUtcNow(),
            //        EntityStatus = EEntityStatus.Active,
            //    });
            //}

            //await _uow.OrderItems.AddRangeAsync(orderItems);

            //var addOrderItemsResult = await _uow.OrderItems.FindManyAsync(oi => oi.OrderId == order.Id);

            //foreach (var item in addOrderItemsResult)
            //{
            //    var orderItemResponseModel = new OrderItemResponseModel
            //    {
            //        OrderId = orderId,
            //        ProductVariantOptionId = item.Product.PublicId,
            //        ProductName = item.Product.Name,
            //        CategoryName = item.Product.Category.Name,
            //        MainImageUrl = item.Product.MainImageUrl,
            //        Quantity = item.Quantity,
            //        PriceAtOrderTime = item.PriceAtOrderTime,
            //        Discount = item.Discount,
            //        TotalPrice = item.TotalPrice
            //    };

            //    serviceResult.Data.Items.Add(orderItemResponseModel);
            //}

            //var result = await _uow.CommitAsync();
            //if (result < 1)
            //{
            //    serviceResult.Message = Messenger.SystemError;
            //    return serviceResult;
            //}

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }

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

            var order = await _uow.Orders.GetOrderIncludeItemsAsync(o => o.PublicId == id);

            if (order == null)
            {
                return serviceResult;
            }

            var payment = await _uow.Payments.FindOneAsync(p => p.OrderId == order.Id);

            if (payment == null)
            {
                return serviceResult;
            }

            if (order.OrderStatus == EOrderStatus.Completed)
            {
                serviceResult.Data = order.ToInStoreOrderResponseModel(order.OrderItems.ToList(), null, payment);
            }
            else
            {
                var paymentQRCode = await _uow.QRCodes.FindOneAsync(q => q.RelatedId ==payment.Id && q.Type == EQRCodeType.Payment);

                if (order.OrderItems.ToList().Count < 1 || paymentQRCode == null)
                {
                    return serviceResult;
                }

                serviceResult.Data = order.ToInStoreOrderResponseModel(order.OrderItems.ToList(), paymentQRCode, payment);
            }


            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;
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

            var order = await _uow.Orders.GetByIdAsync(id);

            if (order == null)
            {
                return serviceResult;
            }

            var invoice = await _uow.Invoices.FindOneAsync(i => i.OrderId == order.Id);

            var payment = await _uow.Payments.FindOneAsync(p => p.OrderId == order.Id);

            order.OrderStatus = EOrderStatus.Completed;
            order.UpdatedAt = TimeZoneHelper.GetUtcNow();

            if (invoice != null)
            {
                invoice.InvoiceStatus = EInvoiceStatus.Paid;
                invoice.UpdatedAt = TimeZoneHelper.GetUtcNow();
                invoice.PaidAt = TimeZoneHelper.GetUtcNow();
                _uow.Invoices.Update(invoice);
            }

            if (payment != null)
            {
                payment.PaymentStatus = EPaymentStatus.Paid;
                payment.UpdatedAt = TimeZoneHelper.GetUtcNow();
                payment.TransactionCode = Random.Shared.Next(100000, 999999).ToString();
                _uow.Payments.Update(payment);
            }

            var result = await _uow.CommitAsync();
            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;
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

            var order = await _uow.Orders.GetByIdAsync(id);

            if (order == null)
            {
                return serviceResult;
            }

            var invoice = await _uow.Invoices.FindOneAsync(i => i.OrderId == order.Id);

            var payment = await _uow.Payments.FindOneAsync(p => p.OrderId == order.Id);


            order.OrderStatus = EOrderStatus.Completed;
            order.PaymentMethod = EPaymentMethod.Cash;
            order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            _uow.Orders.Update(order);

            if (invoice != null)
            {
                invoice.InvoiceStatus = EInvoiceStatus.Paid;
                invoice.PaidAt = TimeZoneHelper.GetUtcNow();
                invoice.UpdatedAt = TimeZoneHelper.GetUtcNow();
                _uow.Invoices.Update(invoice);
            }

            if(payment != null)
            {
                payment.PaymentStatus = EPaymentStatus.Paid;
                payment.UpdatedAt = TimeZoneHelper.GetUtcNow();
                payment.PaymentMethod = EPaymentMethod.Cash;
                _uow.Payments.Update(payment);
            }

            var result = await _uow.CommitAsync();
            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.Data = true;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;
            return serviceResult;
        }

        public async Task<ServiceResult<List<OrderListItemModel>>> GetCustomerOrdersAsync(string customerId)
        {
            var serviceResult = new ServiceResult<List<OrderListItemModel>>
            {
                IsSuccess = true,
                Data = new List<OrderListItemModel>(),
                Message = Messenger.GetDataSuccessful,
            };

            var customer = await _uow.Users.GetByIdAsync(customerId);

            if (customer == null)
            {
                serviceResult.IsSuccess = false;
                serviceResult.Message = Messenger.NotFoundUser;
                return serviceResult;
            }

            var listOrders = await _uow.Orders.GetOrdersIncludeItemsDetailAsync(o => o.CustomerId == customer.Id);

            if (listOrders == null) {
                return serviceResult;
            }

            foreach (var order in listOrders)
            {
                var orderResponseModel = order.ToOrderListItemModel();
                serviceResult.Data.Add(orderResponseModel);
            }

            return serviceResult;
        }

    }
}
