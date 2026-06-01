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
        public string? Name { get; set; }
        public required string Description { get; set; }

        public int? Stock { get; set; }
        public decimal? Price { get; set; }
        public decimal? ImportPrice { get; set; }

        public int? SoldCount { get; set; }

        public decimal? SalePrice { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleEnd { get; set; }
    }
}
