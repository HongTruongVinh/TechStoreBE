using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Ai.Ver2
{
    using System.Text.Json;

    public class AiFunctionCall
    {
        public string Name { get; set; } = string.Empty;

        public string CallId { get; set; } = string.Empty;

        public JsonElement Arguments { get; set; }
    }
}
