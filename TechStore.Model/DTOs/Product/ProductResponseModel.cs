using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.Brand;
using TechStore.Model.DTOs.Category;

namespace TechStore.Model.DTOs.Product
{
    public class ProductResponseModel
    {
        public required string ProductId { get; set; }

        public required string Name { get; set; }

        public required string ShortDescription { get; set; }

        public required string Description { get; set; } = "";

        public required int Stock { get; set; }

        public required decimal Price { get; set; } = 0;

        public required CategoryResponseModel Category { get; set; }

        public required BrandResponseModel Brand { get; set; }

        public required long AverageRating { get; set; }

        public required int SoldCount { get; set; }

        public required DateTime StartSellingDate { get; set; }

        public required string MainImageUrl { get; set; }

        public List<string>? GalleryImageUrls { get; set; } = new();

        public required decimal SalePrice { get; set; }

        public DateTime? SaleStart { get; set; }

        public DateTime? SaleEnd { get; set; }

        public required bool IsOnSale { get; set; }

        public required bool IsFeatured { get; set; }
    }
}
