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
        public string Name { get; set; } = ""; // variant name

        public decimal Price { get; set; } = 0;
        public decimal ImportPrice { get; set; } = 0;

        public int SoldCount { get; set; } = 0;

        public decimal? SalePrice { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleEnd { get; set; }

        public ICollection<ProductVariantOption> Options { get; set; } = new List<ProductVariantOption>();
    }
}
