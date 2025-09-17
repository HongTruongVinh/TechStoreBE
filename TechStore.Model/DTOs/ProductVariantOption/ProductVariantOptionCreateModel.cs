using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.ProductVariantOption
{
    public class ProductVariantOptionCreateModel
    {
        public string? ProductVariantId { get; set; }

        //Black, Silver, Blue
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
        public int Stock { get; set; } = 0;
        public decimal Price { get; set; } = 0;  // Could override Variant
    }
}
