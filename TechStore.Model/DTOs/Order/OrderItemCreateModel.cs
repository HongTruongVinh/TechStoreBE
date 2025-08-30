using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Order
{
    public class OrderItemCreateModel
    {
        public string? OrderId { get; set; }

        public required string ProductId { get; set; }

        public required int Quantity { get; set; }

        public required decimal PriceAtOrderTime { get; set; }

        public required decimal Discount { get; set; }

        public required decimal TotalPrice { get; set; } = 0; // TotalPrice = PriceAtOrderTime * Quantity - Discoun
    }
}
