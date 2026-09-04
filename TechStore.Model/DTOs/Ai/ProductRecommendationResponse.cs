using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Ai
{
    public class ProductRecommendationResponse
    {
        public string Summary { get; set; } = string.Empty;

        public List<ProductRecommendation> Recommendations { get; set; } = [];
    }

    public class ProductRecommendation
    {
        public required string ProductId { get; set; }

        public required string Slug { get; set; }

        public required string ImgUrl { get; set; }

        public required string ProductName { get; set; }

        public required decimal Price { get; set; }

        public int Rank { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}
