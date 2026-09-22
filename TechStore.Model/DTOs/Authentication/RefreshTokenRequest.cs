using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Authentication
{
    public class RefreshTokenRequest
    {
        public required string IdempotencyKey { get; set; }
        public required string RefreshToken { get; set; }
    }
}
