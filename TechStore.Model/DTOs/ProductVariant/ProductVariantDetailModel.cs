using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.ProductVariantOption;

namespace TechStore.Model.DTOs.ProductVariant
{
    public class ProductVariantDetailModel
    {
        public required string Id { get; set; }
        public required string ProductId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public int? Stock { get; set; }
        public required decimal Price { get; set; }
        public required int SoldCount { get; set; }

        public decimal SalePrice { get; set; } = 0;
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleEnd { get; set; }

        public required List<ProductVariantOptionDetailModel> Options { get; set; }
    }
}
