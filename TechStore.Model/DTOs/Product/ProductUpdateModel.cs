using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.ProductVariant;

namespace TechStore.Model.DTOs.Product
{
    public class ProductUpdateModel
    {
        public required string Name { get; set; }
        public required string CategoryId { get; set; }
        public required string BrandId { get; set; }

        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }

        public List<string> Tags { get; set; } = [];
        public required string MainImageUrl { get; set; }
        public List<string> GalleryImageUrls { get; set; } = [];

        public DateTime? StartSellingDate { get; set; }
        public DateTime? EndSellingDate { get; set; }
        public bool? IsFeatured { get; set; }

        public decimal? SalePrice { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleEnd { get; set; }
        public DateTime? PublishDate { get; set; }

        //public List<ProductVariantUpdateModel>? Variants { get; set; }
    }
}
