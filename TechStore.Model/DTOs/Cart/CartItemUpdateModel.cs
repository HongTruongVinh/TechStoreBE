using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Cart
{
    public class CartItemUpdateModel
    {
        public required string ProductVariantOptionId { get; set; }
        public required int Quantity { get; set; } = 0;
    }
}
