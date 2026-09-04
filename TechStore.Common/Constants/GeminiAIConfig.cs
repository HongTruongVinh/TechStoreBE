using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Common.Constants
{
    public class GeminiAIConfig
    {
        public string ApiKey { get; set; } = null!;
        public string Model { get; set; } = "gemini-3.5-flash-lite";
    }
}
