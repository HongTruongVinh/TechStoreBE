using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Order
{
    public class OrderItemResponseModel
    {
        public required string ProductVariantOptionId { get; set; }

        public required string ProductName { get; set; }
        public required string CategoryName { get; set; }
        public required string ImageUrl { get; set; }

        public required int Quantity { get; set; }
        public required decimal PriceAtOrderTime { get; set; }
        public required decimal TotalPrice { get; set; } // TotalPrice = PriceAtOrderTime * Quantity - Discoun

    }
}
