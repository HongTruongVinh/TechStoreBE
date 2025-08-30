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
using TechStore.Model.DTOs.Payment;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;

        public PaymentService(IUnitOfWork uow,
            SequenceGeneratorService sequenceService
            )
        {
            _uow = uow;
            _sequenceService = sequenceService;
        }

        public async Task<ServiceResult<string>> AddPayment(PaymentCreateModel paymentCreateModel)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = string.Empty,
                Message = Messenger.BadRequest,
            };

            var order = await _uow.Orders.GetByIdAsync(paymentCreateModel.OrderId);
            if (order == null) 
            {
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            var paymentId = await _sequenceService.GetNextPaymentIdAsync();

            Payment payment = new Payment
            {
                PublicId = paymentId,
                OrderId = order.Id,
                Order = order,
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
    }
}
