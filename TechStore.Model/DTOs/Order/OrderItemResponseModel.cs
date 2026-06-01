using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Order
{
    public class OrderItemResponseModel
    {
        public string? Id { get; set; }
        public string? OrderId { get; set; }
        public required string ProductVariantOptionId { get; set; }

        public required string ProductName { get; set; }
        public required string OptionName { get; set; }
        public required string VariantName { get; set; }
        public required string CategoryName { get; set; }

        public required string MainImageUrl { get; set; }
        public required int Quantity { get; set; }
        public required decimal PriceAtOrderTime { get; set; }

        public required decimal Discount { get; set; }
        public required decimal TotalPrice { get; set; } // TotalPrice = PriceAtOrderTime * Quantity - Discoun

    }
}
