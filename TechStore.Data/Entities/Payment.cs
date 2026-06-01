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
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }

        public required Guid UserId { get; set; }
        public required User User { get; set; }

        public required decimal Amount { get; set; } = 0;

        public required EPaymentMethod PaymentMethod { get; set; }

        public required string TransactionCode { get; set; }

        public required EPaymentStatus PaymentStatus { get; set; }

        public DateTime ExpiredAt { get; set; }

        // Snapshot checkout
        public string? CheckoutSnapshotJson { get; set; }

    }
}
