using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.ProductVariant;

namespace TechStore.Model.DTOs.Product
{
    public class ProductCreateModel
    {
        public required string Name { get; set; }
        public required string CategoryId { get; set; }
        public required string BrandId { get; set; }

        public required string ShortDescription { get; set; }
        public required string Description { get; set; } = "";
        public string? Slug { get; set; }
        public required int Warranty { get; set; }

        public required List<string> Tags { get; set; }
        public string? MainImageUrl { get; set; }
        public required List<string> GalleryImageUrls { get; set; } = new();

        public DateTime? StartSellingDate { get; set; }
        public DateTime? EndSellingDate { get; set; }
        public required bool IsFeatured { get; set; }

        public decimal? SalePrice { get; set; } = 0;
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleEnd { get; set; }
        public DateTime? PublishDate { get; set; }

        public required List<ProductVariantCreateModel> Variants { get; set; }
    }
}
