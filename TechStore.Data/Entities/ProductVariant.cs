using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class ProductVariant : BaseEntity
    {
        public required Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        // Ví dụ: 128GB, 256GB, 512GB
        public required string Name { get; set; }
        public required string Description { get; set; }

        public required decimal Price { get; set; }
        public required decimal ImportPrice { get; set; }
        public int SoldCount { get; set; }


        public ICollection<ProductVariantOption> Options { get; set; } = new List<ProductVariantOption>();
    }
}
