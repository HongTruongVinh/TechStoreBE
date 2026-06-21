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
        public required string Name { get; set; }
        public required string Description { get; set; }

        public required decimal Price { get; set; }
        public required decimal ImportPrice { get; set; }

        public required List<ProductVariantOptionCreateModel> Options { get; set; }

    }
}
