using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.Payment
{
    public class PaymentUpdateModel
    {
        public string? Id { get; set; }

        public required string OrderId { get; set; }

        public required long Amount { get; set; } = 0;

        public required EPaymentStatus PaymentStatus { get; set; }
    }
}
