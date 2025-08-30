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
        public Order? Order { get; set; }

        public required Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public required int Quantity { get; set; }

        public required decimal PriceAtOrderTime { get; set; }

        public required decimal Discount { get; set; }

        public required decimal TotalPrice { get; set; } = 0; // TotalPrice = PriceAtOrderTime * Quantity - Discount
    }
}
