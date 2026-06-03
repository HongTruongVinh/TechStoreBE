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
        public static PaymentResponseModel ToPaymentResponseModel(this Payment payment, Order order, QRCode? qrCode)
        {
            return new PaymentResponseModel
            {
                Id = payment.PublicId,
                OrderId = order.PublicId,
                CustomerId = order.Customer?.PublicId ?? "",
                CustomerName = order.CustomerName,
                CustomerPhonenumber = order.CustomerPhoneNumber,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                TransactionCode = payment.TransactionCode,
                PaymentStatus = payment.PaymentStatus,
                QRCode = qrCode?.ToQRCodeResponseModel(),
                CreatedAt = payment.CreatedAt
            };
        }
    }
}
