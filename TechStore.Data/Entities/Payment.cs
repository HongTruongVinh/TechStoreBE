using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class Payment : BaseEntity
    {
        public required Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;

        public required Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public required decimal Amount { get; set; }
        public required EPaymentMethod PaymentMethod { get; set; }
        public required string PaymentCode { get; set; } // example: STORE20260710158671
        public required string BankReferenceCode { get; set; } // example: 6bea03d5-4acf-66a6-8cd3-10c6789efeb3
        public required string TransactionId { get; set; } // example: 67532173

        public required EPaymentStatus PaymentStatus { get; set; }

        // Snapshot checkout
        public string? CheckoutSnapshotJson { get; set; }

    }
}
