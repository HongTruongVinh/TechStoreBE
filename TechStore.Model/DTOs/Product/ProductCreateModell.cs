using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Product
{
    public class ProductCreateModell
    {
        public required string Name { get; set; }
        public required string CategoryId { get; set; }
        public required string BrandId { get; set; }

        public required string ShortDescription { get; set; }
        public required string Description { get; set; } = "";
        public string? Slug { get; set; }

        public required List<string> Tag { get; set; }
        public string? MainImageUrl { get; set; }
        public List<string>? GalleryImageUrls { get; set; } = new();

        public required int Stock { get; set; }
        public required decimal Price { get; set; } = 0;
        public required decimal ImportPrice { get; set; } = 0;

        public DateTime? StartSellingDate { get; set; }
        public DateTime? EndSellingDate { get; set; }
        public required bool IsFeatured { get; set; }

        public required decimal SalePrice { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleEnd { get; set; }
    }
}
