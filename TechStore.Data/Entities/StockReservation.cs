using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class StockReservation : BaseEntity
    {
        public required Guid ProductVariantOptionId { get; set; }

        public required Guid PaymentSnapshotId { get; set; }

        public required int Quantity { get; set; }

        public required DateTime ReservedAt { get; set; }

        public required DateTime ExpiresAt { get; set; }

        public StockReservationStatus Status { get; set; } = StockReservationStatus.Reserved;

        // Navigation properties
        public ProductVariantOption ProductVariantOption { get; set; } = null!;

        public PaymentSnapshot PaymentSnapshot { get; set; } = null!;
    }
}
