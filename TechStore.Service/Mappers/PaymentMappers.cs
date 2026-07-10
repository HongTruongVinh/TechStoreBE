using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Payment;

namespace TechStore.Service.Mappers
{
    public static class PaymentMappers
    {
        public static PaymentResponseModel ToPaymentResponseModel(this Payment payment)
        {
            return new PaymentResponseModel
            {
                Id = payment.PublicId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                TransactionCode = payment.PaymentCode,
                PaymentStatus = payment.PaymentStatus,
                CreatedAt = payment.CreatedAt
            };
        }
    }
}
