using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Payment
{
    public class PaymentForSnapshotWebhookRequest
    {
        public required string SnapshotId { get; set; }

        public required decimal Amount { get; set; }

        public required string TransactionId { get; set; }
    }

    public class PaymentForInvocieWebhookRequest
    {
        public required string PaymentId { get; set; }

        public required decimal Amount { get; set; }

        public required string TransactionId { get; set; }
    }
}
