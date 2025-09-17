using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;
using TechStore.Model.DTOs.Brand;
using TechStore.Model.DTOs.Category;
using TechStore.Model.DTOs.ProductVariant;
namespace TechStore.Model.DTOs.Product
{
    public class AdminProductDetailModel
    {
        public required string ProductId { get; set; }

        public required string Name { get; set; }

        public required string ShortDescription { get; set; }

        public required string Description { get; set; } = "";

        //public required int Stock { get; set; }

        //public required decimal Price { get; set; } = 0;

        //public required decimal ImportPrice { get; set; } = 0;

        public DateTime StartSellingDate { get; set; } 

        public DateTime? EndSellingDate { get; set; } 

        public required CategoryResponseModel Category { get; set; }

        public required BrandResponseModel Brand { get; set; }

        public required long AverageRating { get; set; }

        public required int SoldCount { get; set; }

        public required int RatedCount { get; set; }

        public required string Slug { get; set; }

        public required List<string> Tag { get; set; }

        public string? MainImageUrl { get; set; }

        public List<string>? GalleryImageUrls { get; set; } = new();

        public required decimal SalePrice { get; set; }

        public DateTime? SaleStart { get; set; }

        public DateTime? SaleEnd { get; set; }

        public required bool IsOnSale { get; set; }

        public required bool IsFeatured { get; set; }

        public EEntityStatus? EntityStatus { get; set; }

        public required DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public required List<AdminProductVariantDetailModel> Variants { get; set; }
    }
}
