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
                PaymentStatus = EPaymentStatus.Paid,
               

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

            payment.DeletedAt = TimeZoneHelper.GetUtcNow();
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
                Amount = model.Amount,
                CreatedBy = cashier.Id,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
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

        public async Task<ServiceResult<PaymentDataForSnapshotModel>> GenerateSnapshotPaymentQrAsync(string userId, string snapshotId)
        {
            var now = TimeZoneHelper.GetUtcNow();
            var customer = await _uow.Users.TableNoTracking
                .Where(u => u.PublicId == userId)
                .Select(u => new { u.Id })
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.NotFound, Messenger.NotFoundUser);
            }

            var snapshot = await _uow.PaymentSnapshots.TableNoTracking
                .Where(x =>
                    x.PublicId == snapshotId &&
                    x.CustomerId == customer.Id &&
                    x.Status == EPaymentSnapshotStatus.PendingPayment &&
                    x.ExpiredAt > now)
                .Select(x => new
                {
                    x.PublicId,
                    x.TotalAmount,
                    x.ExpiredAt
                })
                .FirstOrDefaultAsync();

            if (snapshot == null)
            {
                return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.NotFound, Messenger.NoExitData);
            }

            var paymentQrUrl = await _vietQrService.GenerateQrAsync(snapshot.TotalAmount, snapshot.PublicId);

            if (paymentQrUrl == null)
            {
                return ServiceResult<PaymentDataForSnapshotModel>.Fail(EErrorType.Status500InternalServerError, Messenger.SystemError);
            }

            var paymentData = new PaymentDataForSnapshotModel()
            {
                SnapshotId = snapshot.PublicId,
                QrDataURL = paymentQrUrl,
                Amount = snapshot.TotalAmount,
                CreatedAt = now,
                ExpiredAt = snapshot.ExpiredAt,
            };

            return ServiceResult<PaymentDataForSnapshotModel>.Success(paymentData);
        }
    }
}
