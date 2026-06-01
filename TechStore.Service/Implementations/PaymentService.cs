using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;
        private readonly IVietQrService _vietQrService;
        private readonly IOrderService _orderService;

        public PaymentService(IUnitOfWork uow,
            SequenceGeneratorService sequenceService,
            IVietQrService vietQrService,
            IOrderService orderService
            )
        {
            _uow = uow;
            _sequenceService = sequenceService;
            _vietQrService = vietQrService;
            _orderService = orderService;
        }

        public async Task<ServiceResult<string>> CreatePaymentByCustomer(string userId, PaymentCreateModel paymentCreateModel)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = string.Empty,
                Message = Messenger.BadRequest,
            };
            //if (order == null) 
            //{
            //    serviceResult.Message = Messenger.NoExitData;
            //    return serviceResult;
            //}

            var customer = await _uow.Users.GetByIdAsync(userId);
            if (customer == null)
            {
                serviceResult.Message = Messenger.NoExitData + " " + userId;
                return serviceResult;
            }

            var paymentId = await _sequenceService.GetNextPaymentIdAsync();

            Payment payment = new Payment
            {
                PublicId = paymentId,
                UserId = customer.Id,
                User = customer,
                Amount = paymentCreateModel.Amount,
                PaymentMethod = paymentCreateModel.PaymentMethod,
                TransactionCode = paymentCreateModel.TransactionCode,
                PaymentStatus = EPaymentStatus.Paid,

                CreatedAt = TimeZoneHelper.GetUtcNow(),
                EntityStatus = EEntityStatus.Active,
            };

            await _uow.Payments.AddAsync(payment);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.Data = paymentId;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;
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

            _uow.Payments.Remove(payment);
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

            List<PaymentResponseModel> paymentResponseModels = new List<PaymentResponseModel>();

            foreach (var payment in payments)
            {
                var order = await _uow.Orders.FindOneAsync(o => o.Id == payment.OrderId);

                if (order != null)
                {
                    serviceResult.Data.Add(payment.ToPaymentResponseModel(order, null));
                }
            }

            serviceResult.Data = paymentResponseModels;

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

            var order = await _uow.Orders.FindOneAsync(o => o.Id == payment.OrderId);

            if (order == null)
            {
                return serviceResult;
            }

            serviceResult.Data = payment.ToPaymentResponseModel(order, null);
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> CheckoutPayment(string paymentId)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.SystemError,
            };

            var payment = await _uow.Payments.GetByIdAsync(paymentId);

            if (payment == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var order = await _uow.Orders.FindOneAsync(o => o.Id == payment.OrderId);

            if (order == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            if (order.Invoice == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var invoice = await _uow.Invoices.FindOneAsync(i => i.Id == order.Invoice.Id);
            if (invoice == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            order.OrderStatus = EOrderStatus.Completed;
            invoice.InvoiceStatus = EInvoiceStatus.Paid;
            payment.PaymentStatus = EPaymentStatus.Paid;

            order.UpdatedAt = TimeZoneHelper.GetUtcNow();
            invoice.UpdatedAt = TimeZoneHelper.GetUtcNow();
            payment.UpdatedAt = TimeZoneHelper.GetUtcNow();

            payment.TransactionCode = $"PAY-{DateTime.UtcNow:yyyyMMddHHmmss}"; // Example transaction code format

            _uow.Orders.Update(order);
            _uow.Invoices.Update(invoice);
            _uow.Payments.Update(payment);

            var updateResult = await _uow.CommitAsync();
            if (updateResult < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<string>> VerifyPayment(PaymentWebhookRequest request)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = string.Empty,
                Message = Messenger.SystemError,
            };

            var payment = await _uow.Payments.GetByIdAsync(request.PaymentId);

            if (payment == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            if(payment.CheckoutSnapshotJson == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var snapshot = JsonSerializer.Deserialize<CheckoutSnapshot>(payment.CheckoutSnapshotJson);

            if (snapshot == null)
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var orderCreateModel = new OrderCreateModel
            {
                CustomerName = snapshot.CustomerName,
                CustomerEmail = snapshot.CustomerEmail,
                CustomerPhoneNumber = snapshot.CustomerPhoneNumber,
                ShippingAddress = snapshot.ShippingAddress,
                Note = snapshot.Note,
                PaymentMethod = payment.PaymentMethod,
                Items = snapshot.Items.Select(i => new OrderItemCreateModel
                {
                    ProductVariantOptionId = i.ProductVariantOptionId,
                    Quantity = i.Quantity,
                    Discount = i.Discount
                }).ToList()
            };

            // VERIFY SIGNATURE HERE

            // VERIFY AMOUNT
            if (request.Amount != payment.Amount)
            {
                serviceResult.Message = Messenger.IncorrectAmount;
                return serviceResult;
            }

            // VERIFY TRANSACTION HERE

            // FIND PAYMENT

            // UPDATE PAYMENT STATUS
            payment.PaymentStatus = EPaymentStatus.Paid;
            //payment.TransactionCode = request.TransactionId;
            payment.TransactionCode = request.TransactionId + Random.Shared.Next(10000, 99999);

            // UPDATE ORDER STATUS
            var createOrderesult = await _orderService.CreatePrePayOnlineOrderAsync(snapshot.CustomerId, request.PaymentId, orderCreateModel);

            if (createOrderesult.IsSuccess == false)
            {
                serviceResult.ErrorCode = createOrderesult.ErrorCode;
                serviceResult.Message = createOrderesult.Message;
                return serviceResult;
            }

            _uow.Payments.Update(payment);
            var updateResult = await _uow.CommitAsync();
            if (updateResult < 1)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            if (payment.PaymentStatus == EPaymentStatus.Paid)
            {
                serviceResult.IsSuccess = true;
                serviceResult.Data = payment.TransactionCode;
                serviceResult.Message = Messenger.PaymentVerified;
                return serviceResult;
            }
            else
            {
                serviceResult.Message = Messenger.PaymentNotVerified;
                return serviceResult;
            }
        }

        public async Task<ServiceResult<PaymentDataModel>> CreatePaymentForPrepayOrder(string userId, OrderCreateModel orderCreateModel)
        {
            var serviceResult = new ServiceResult<PaymentDataModel>
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

            var paymentId = await _sequenceService.GetNextPaymentIdAsync();

            // Tính toán tổng tiền
            decimal totalPrice = 0;
            decimal totalDiscount = 0;

            var snapshot = new CheckoutSnapshot
            {
                CustomerId = customer.PublicId,
                CustomerName = orderCreateModel.CustomerName,
                CustomerPhoneNumber = orderCreateModel.CustomerPhoneNumber,
                CustomerEmail = orderCreateModel.CustomerEmail,
                TotalPrice = 0,
                ShippingCharge = 0,
                DiscountAmount = totalDiscount,
                FinalAmount = 0,
                ShippingAddress = orderCreateModel.ShippingAddress,
                Note = orderCreateModel.Note,
                Items = new List<CheckoutSnapshotItem>()
            };

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
                    snapshot.Items.Add(new CheckoutSnapshotItem
                    {
                        ProductVariantOptionId = productVariantOption.PublicId,
                        Quantity = item.Quantity,
                        PriceAtOrderTime = productVariantOption.ProductVariant.Price,
                        Discount = item.Discount,
                        TotalPrice = item.Quantity * productVariantOption.ProductVariant.Price
                    });
                }

                decimal itemTotal = item.Quantity * productVariantOption.ProductVariant.Price;
                totalPrice += itemTotal;
                totalDiscount += item.Discount;
            }

            var finalAmount = totalPrice - totalDiscount;
            snapshot.TotalPrice = totalPrice;
            snapshot.DiscountAmount = totalDiscount;
            snapshot.FinalAmount = finalAmount;

            var payment = new Payment
            {
                PublicId = paymentId,
                UserId = customer.Id,
                User = customer,
                PaymentMethod = orderCreateModel.PaymentMethod,
                Amount = finalAmount,
                TransactionCode = "",
                ExpiredAt = TimeZoneHelper.GetUtcNow().AddMinutes(20),
                PaymentStatus = EPaymentStatus.Pending,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
                EntityStatus = EEntityStatus.Active,
                CheckoutSnapshotJson = JsonSerializer.Serialize(snapshot)
            };

            await _uow.Payments.AddAsync(payment);

            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            var paymentQrUrl = await _vietQrService.GenerateQrAsync(payment.Amount, $"TECHSTORE {payment.PublicId}");

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
                CreatedAt = payment.CreatedAt,
                ExpiredAt = payment.ExpiredAt,
            };

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }
    }
}
