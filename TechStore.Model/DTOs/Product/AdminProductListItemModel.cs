using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Product
{
    public class AdminProductListItemModel
    {
        public required string ProductId { get; set; }
        public required string Name { get; set; }
        public required string MainImageUrl { get; set; } 

        public string? CategoryName { get; set; }
        public int? Stock { get; set; }
        public decimal? Price { get; set; } = 0;

        public long? AverageRating { get; set; }
        public int? SoldCount { get; set; }
        public int? RatedCount { get; set; }

        public DateTime? StartSellingDate { get; set; }

    }
}
