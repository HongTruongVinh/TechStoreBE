using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.ProductVariantOption
{
    public class ProductVariantOptionUpdateModel
    {
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public required int Stock { get; set; }
        public required decimal Price { get; set; }
        public required decimal ImportPrice { get; set; }
    }
}
