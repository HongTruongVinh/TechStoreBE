using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.ProductVariantOption
{
    public class ProductVariantOptionCreateModel
    {
        //Black, Silver, Blue
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public int Stock { get; set; } = 0;
        public decimal? Price { get; set; } = 0;  // Could override Variant
    }
}
