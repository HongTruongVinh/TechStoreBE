using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class Product : BaseEntity
    {
        public required Guid CategoryId { get; set; }
        public required string CategoryPublicId { get; set; }
        public Category Category { get; set; } = null!;
        public required Guid BrandId { get; set; }
        public required string BrandPublicId { get; set; }
        public Brand Brand { get; set; } = null!;

        public required string Name { get; set; }
        public required string ShortDescription { get; set; } = "";
        public required string Description { get; set; } = "";
        public required int Warranty { get; set; }

        public required string Slug { get; set; }
        public required List<string> Tags { get; set; }
        public required string MainImageUrl { get; set; }
        public required List<string> GalleryImageUrls { get; set; } = new();


        public DateTime StartSellingDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndSellingDate { get; set; }
        public required bool IsFeatured { get; set; }

        public long AverageRating { get; set; } = 0;
        public int TotalReviews { get; set; } = 0;
        public int SoldCount { get; set; } = 0;
        public int RatedCount { get; set; } = 0;

        public int ViewCount { get; set; } = 0;

        public required decimal SalePrice { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleEnd { get; set; }
        public DateTime? PublishDate { get; set; }

        public bool IsOnSale => SaleStart <= DateTime.Now && SaleEnd >= DateTime.Now;

        public required decimal MinPrice { get; set; }

        public required decimal MaxPrice { get; set; }

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}
