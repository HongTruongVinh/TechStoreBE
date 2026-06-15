using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Cart
{
    public class CartItemResponseModel
    {
        public required string Id { get; set; }
        public required string ProductId { get; set; }
        public required string ProductVariantOptionId { get; set; }

        public required string ProductName { get; set; }    
        public required string VariantName { get; set; }
        public required string OptionName { get; set; }

        public required string MainImageUrl { get; set; }
        public required int Quantity { get; set; }
        public required decimal Price { get; set; }

        public required decimal Discount { get; set; }
        public required decimal TotalPrice { get; set; } = 0; // TotalPrice = PriceAtOrderTime * Quantity - Discoun
        public required string Slug { get; set; }
        public required int Stock { get; set; }
    }
}
