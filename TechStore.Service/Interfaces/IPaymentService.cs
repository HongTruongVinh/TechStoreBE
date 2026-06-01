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
        Task<ServiceResult<string>> CreatePaymentByCustomer(string userId, PaymentCreateModel paymentModel);
        Task<ServiceResult<bool>> DeletePayment(string paymentId);
        Task<ServiceResult<List<PaymentResponseModel>>> GetPayments();
        Task<ServiceResult<PaymentResponseModel>> GetPayment(string paymentId);
        Task<ServiceResult<PaymentDataModel>> CreatePaymentForPrepayOrder(string userId, OrderCreateModel orderCreateModel);
        Task<ServiceResult<bool>> CheckoutPayment(string paymentId);

        Task<ServiceResult<string>> VerifyPayment(PaymentWebhookRequest request);
    }
}
