using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Payment
{
    public class PaymentForSnapshot
    {
        public required decimal Amount { get; set; }

        public required string Code { get; set; } // example: STORE20260710158671
        public required string BankReferenceCode { get; set; } // example: 6bea03d5-4acf-66a6-8cd3-10c6789efeb3
        public required string TransactionId { get; set; } // example: 67532173
    }

    public class PaymentForInvocieWebhookRequest
    {
        public required string PaymentId { get; set; }

        public required decimal Amount { get; set; }

        public required string TransactionId { get; set; }
    }
}
