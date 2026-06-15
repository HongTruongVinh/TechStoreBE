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
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public int Stock { get; set; }

        public required decimal Price { get; set; }
        public required decimal ImportPrice { get; set; }
        public int SoldCount { get; set; }
    }
}
