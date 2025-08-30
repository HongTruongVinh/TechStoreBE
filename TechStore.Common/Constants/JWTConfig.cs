using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Common.Constants
{
    public class JWTConfig
    {
        public string SigningKey { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpireMinutes { get; set; } = 60;
    }
}
