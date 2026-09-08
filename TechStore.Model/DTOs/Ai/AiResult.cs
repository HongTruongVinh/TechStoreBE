using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Ai
{
    public class AiResult
    {
        public required string InteractionId { get; set; }

        public int? InputTokens { get; set; }

        public int? OutputTokens { get; set; }

        public required ProductRecommendationResponse Recommendation { get; set; }
    }
}
