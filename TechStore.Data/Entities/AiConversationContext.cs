using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class AiConversationContext : BaseEntity
    {
        public required Guid ConversationId { get; set; }
        public AiConversation Conversation { get; set; } = null!;

        public required string ConversationPublicId { get; set; }

        public string? Category { get; set; }
        public string? Brand { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public List<string> Usages { get; set; } = [];

        public List<string> Games { get; set; } = [];

        public List<string> LastRecommendedProductIds { get; set; } = [];
    }
}
