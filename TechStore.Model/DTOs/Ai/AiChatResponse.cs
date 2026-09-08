using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Ai
{
    public class AiChatResponse
    {
        public required string ConversationId { get; set; }

        public required string Content { get; set; }

        public List<ProductRecommendation> Recommendations { get; set; } = [];

        public string? GuestId { get; set; }
    }
}
