using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;
using TechStore.Model.DTOs.QrCode;

namespace TechStore.Model.DTOs.Payment
{
    public class PaymentResponseModel
    {
        public required string Id { get; set; }

        public string? CustomerId { get; set; }

        public required string CustomerName { get; set; }

        public required string CustomerPhonenumber { get; set; }

        public required string OrderId { get; set; }

        public required decimal Amount { get; set; } = 0;

        public required EPaymentMethod PaymentMethod { get; set; }

        public required string TransactionCode { get; set; }

        public required EPaymentStatus PaymentStatus { get; set; }

        public QRCodeResponseModel? QRCode { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
