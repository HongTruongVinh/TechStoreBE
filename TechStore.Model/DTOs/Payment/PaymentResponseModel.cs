using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.Payment
{
    public class PaymentResponseModel
    {
        public required string Id { get; set; }
        public string? InvoiceId { get; set; }
        //public required string UserId { get; set; }

        public required decimal Amount { get; set; }

        public string? TransactionCode { get; set; }
        public required EPaymentMethod PaymentMethod { get; set; }
        public required EPaymentStatus PaymentStatus { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
