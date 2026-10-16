using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Ai.Ver2
{
    public class AiToolResult
    {
        public string FunctionName { get; set; } = string.Empty;

        public string CallId { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public object? Result { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
