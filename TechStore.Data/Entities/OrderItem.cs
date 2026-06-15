using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class OrderItem : BaseEntity
    {
        public required Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public required Guid ProductVariantOptionId { get; set; }
        public required string ProductVariantOptionPublicId { get; set; }
        public ProductVariantOption ProductVariantOption { get; set; } = null!;

        public required string CategoryName { get; set; }
        public required string ProductName { get; set; }
        public required string ImageUrl { get; set; }

        public required int Quantity { get; set; }
        public required decimal PriceAtOrderTime { get; set; }
        public required decimal TotalPrice { get; set; } // TotalPrice = PriceAtOrderTime * Quantity
    }
}
