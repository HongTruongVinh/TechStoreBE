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
        public required string TransactionCode { get; set; }

        public required EPaymentStatus PaymentStatus { get; set; }

        // Snapshot checkout
        public string? CheckoutSnapshotJson { get; set; }

    }
}
