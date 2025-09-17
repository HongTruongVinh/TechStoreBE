using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class ProductVariantOption : BaseEntity
    {
        public required Guid ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;

        //Black, Silver, Blue
        public string Name { get; set; } = ""; //option name
        public required string ImageUrl { get; set; }
        public int Stock { get; set; } = 0;
        public decimal Price { get; set; } = 0;  // Could override Variant
    }
}
