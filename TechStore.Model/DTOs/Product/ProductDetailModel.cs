using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.ProductVariant;

namespace TechStore.Model.DTOs.Product
{
    public class ProductDetailModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string ShortDescription { get; set; }

        public required string Description { get; set; } = "";
        public required string MainImageUrl { get; set; }
        public required List<string> GalleryImageUrls { get; set; } = new();

        //public required int Stock { get; set; }
        //public required decimal Price { get; set; } = 0;
        public required int SoldCount { get; set; }
        public required int Warranty { get; set; }

        public required decimal SalePrice { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleEnd { get; set; }

        public required string CategoryId { get; set; }
        public required string CategoryName { get; set; }

        public required string BrandId { get; set; }
        public required string BrandName { get; set; }

        public required int RatedCount { get; set; }
        public required long AverageRating { get; set; }

        public required string Slug { get; set; }
        public List<string>? Tags { get; set; }

        public required bool IsOnSale { get; set; }
        public required bool IsFeatured { get; set; }
        public DateTime? PublishDate { get; set; }

        public required List<ProductVariantDetailModel> Variants { get; set; }
    }
}
