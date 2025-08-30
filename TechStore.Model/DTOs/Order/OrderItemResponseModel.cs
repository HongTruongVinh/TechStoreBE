using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Order
{
    public class OrderItemResponseModel
    {
        public string? OrderItemId { get; set; }

        public string? OrderId { get; set; }

        public required string ProductId { get; set; }

        public required string ProductName { get; set; }

        public required string CategoryName { get; set; }

        public required string MainImageUrl { get; set; }

        public required int Quantity { get; set; }

        public required decimal PriceAtOrderTime { get; set; }

        public required decimal Discount { get; set; }

        public required decimal TotalPrice { get; set; } = 0; // TotalPrice = PriceAtOrderTime * Quantity - Discoun

    }
}
