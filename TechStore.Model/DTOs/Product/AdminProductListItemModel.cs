using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Product
{
    public class AdminProductListItemModel
    {
        public required string ProductVariantOptionId { get; set; }
        public required string Name { get; set; }
        public required string MainImageUrl { get; set; }

        public required string CategoryName { get; set; }
        public required int Stock { get; set; } = 0;
        public required decimal Price { get; set; } = 0;

        public required long AverageRating { get; set; }
        public required int SoldCount { get; set; }
        public required int RatedCount { get; set; }

        public DateTime? StartSellingDate { get; set; }

    }

    //public class AdminProductListItemModel
    //{
    //    public required string ProductId { get; set; }
    //    public required string Name { get; set; }
    //    public required string MainImageUrl { get; set; } 

    //    public string? CategoryName { get; set; }
    //    public int? Stock { get; set; }
    //    public decimal? Price { get; set; } = 0;

    //    public long? AverageRating { get; set; }
    //    public int? SoldCount { get; set; }
    //    public int? RatedCount { get; set; }

    //    public DateTime? StartSellingDate { get; set; }

    //}
}
