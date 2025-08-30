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
        public required Category Category { get; set; }
        public required Guid BrandId { get; set; }
        public required Brand Brand { get; set; }

        public required string Name { get; set; }
        public required string ShortDescription { get; set; } = "";
        public required string Description { get; set; } = "";

        public required string Slug { get; set; }
        public required List<string> Tag { get; set; }
        public required string MainImageUrl { get; set; }
        public List<string>? GalleryImageUrls { get; set; } = new();


        public required int Stock { get; set; }
        public required decimal Price { get; set; } = 0;
        public required decimal ImportPrice { get; set; } = 0;


        public DateTime StartSellingDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndSellingDate { get; set; }
        public required bool IsFeatured { get; set; }

        public long AverageRating { get; set; } = 0;
        public int TotalReviews { get; set; } = 0;
        public int SoldCount { get; set; } = 0;
        public int RatedCount { get; set; } = 0;

        public required decimal SalePrice { get; set; }
        public DateTime? SaleStart { get; set; }
        public DateTime? SaleEnd { get; set; }

        public bool IsOnSale => SaleStart <= DateTime.Now && SaleEnd >= DateTime.Now;
    }
}
