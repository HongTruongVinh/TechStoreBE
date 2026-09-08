using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Ai
{
    public class AiChatRequest
    {
        public required string Message { get; set; }

        public string? ConversationId { get; set; }
    }
}
