using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.ProductVariantOption;

namespace TechStore.Model.DTOs.ProductVariant
{
    public class ProductVariantCreateModel
    {
        public string? ProductId { get; set; }

        public required string Name { get; set; }

        public int Stock { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public decimal ImportPrice { get; set; } = 0;

        public int SoldCount { get; set; } = 0;

        public decimal? SalePrice { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleEnd { get; set; }

        public required List<ProductVariantOptionCreateModel> Options { get; set; }

    }
}
