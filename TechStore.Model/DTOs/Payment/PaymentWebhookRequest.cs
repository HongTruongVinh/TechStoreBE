using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Payment
{
    public class PaymentWebhookRequest
    {
        public required string PaymentId { get; set; }

        public required decimal Amount { get; set; }

        public required string TransactionId { get; set; }
    }
}
