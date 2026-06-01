using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.Payment
{
    public class PaymentCreateModel
    {
        public string? OrderId { get; set; }

        public required long Amount { get; set; } = 0;

        public required EPaymentMethod PaymentMethod { get; set; }

        public required string TransactionCode { get; set; }

        public required EPaymentStatus PaymentStatus { get; set; }
    }
}
