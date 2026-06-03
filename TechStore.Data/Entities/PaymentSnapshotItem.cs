using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class PaymentSnapshotItem : BaseEntity
    {
        public required Guid PaymentSnapshotId { get; set; }
        public required PaymentSnapshot PaymentSnapshot { get; set; }

        public required string ProductVariantOptionId { get; set; }
        public required int Quantity { get; set; }
        public required decimal PriceAtOrderTime { get; set; }
        public required decimal Discount { get; set; }
        public required decimal TotalPrice { get; set; } = 0; // TotalPrice = PriceAtOrderTime * Quantity - Discount
    }
}
