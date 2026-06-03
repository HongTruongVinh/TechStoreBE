using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Product
{
    public class ProductListItemModel
    {
        public required string id { get; set; }
        public required string ProductVariantId { get; set; }
        public required string ProductName { get; set; }
        public required string ProductVariantName { get; set; }
        public required string Slug { get; set; }
        public required int Warranty { get; set; }


        public required string MainImageUrl { get; set; }
        public string? CategoryName { get; set; }
        public required decimal Price { get; set; }
        public required decimal SalePrice { get; set; }

        public long? AverageRating { get; set; }
        public required int SoldCount { get; set; }
        public int? RatedCount { get; set; }

        public DateTime? StartSellingDate { get; set; }
    }
}
