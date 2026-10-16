using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Ai.Ver2
{
    public class AiResponseV2
    {
        public required string InteractionId { get; set; }

        public string? Text { get; set; }

        public List<AiFunctionCall> FunctionCalls { get; set; } = [];


        public int? InputTokens { get; set; }

        public int? OutputTokens { get; set; }
    }
}
