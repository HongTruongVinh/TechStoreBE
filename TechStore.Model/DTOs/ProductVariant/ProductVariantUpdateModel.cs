using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.ProductVariantOption;

namespace TechStore.Model.DTOs.ProductVariant
{
    public class ProductVariantUpdateModel
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? ImportPrice { get; set; }

        public List<ProductVariantOptionUpdateModel>? Options { get; set; }
    }
}
