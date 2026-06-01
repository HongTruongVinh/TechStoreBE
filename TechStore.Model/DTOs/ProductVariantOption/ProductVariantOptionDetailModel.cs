using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.ProductVariantOption
{
    public class ProductVariantOptionDetailModel
    {
        public required string Id { get; set; }
        public required string ProductVariantId { get; set; }
        public required string Name { get; set; }

        public required string ImageUrl { get; set; }
        public required int Stock { get; set; }
        public decimal? Price { get; set; }
    }
}
