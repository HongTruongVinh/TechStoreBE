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

        public required Guid ProductVariantOptionId { get; set; }
        public required string ProductVariantOptionPublicId { get; set; }

        public required string CategoryName { get; set; }
        public required string ProductName { get; set; }
        public required string UrlImage { get; set; }

        public required int Quantity { get; set; }
        public required decimal PriceAtOrderTime { get; set; }
        public required decimal TotalPrice { get; set; } // TotalPrice = PriceAtOrderTime * Quantity
    }
}
