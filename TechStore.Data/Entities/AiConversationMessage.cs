using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class AiConversationMessage : BaseEntity
    {
        public Guid ConversationId { get; set; }
        public AiConversation Conversation { get; set; } = null!;

        public string Role { get; set; } = string.Empty;
        // user | assistant

        public string Content { get; set; } = string.Empty;

        public string? InteractionId { get; set; } // Gemini interaction ID, used to continue the conversation context

        public int? InputTokens { get; set; }

        public int? OutputTokens { get; set; }
    }
}
