using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Payment;

namespace TechStore.Service.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResult<string>> AddCashPaymentByAdminAsync(string cashierId, CashPaymentCreateModel cashPayment);
        Task<ServiceResult<bool>> DeletePayment(string paymentId);
        Task<ServiceResult<List<PaymentResponseModel>>> GetPayments();
        Task<ServiceResult<PaymentResponseModel>> GetPayment(string paymentId);
        Task<ServiceResult<PaymentDataForSnapshotModel>> CreatePaymentForSnapshotAsync(string userId, OrderCreateModel orderCreateModel);
        Task<ServiceResult<PaymentDataModel>> CreatePaymentForInvoiceByAdminAsync(string cashierId, PaymentCreateModel model);

        Task<ServiceResult<VerifyResult>> VerifyPaymentForSnapshotAsync(SepayWebhookRequest request);
        Task<ServiceResult<VerifyResult>> VerifyPaymentForInvoiceAsync(PaymentForInvocieWebhookRequest request);
    }
}
