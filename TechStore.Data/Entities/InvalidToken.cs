using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class InvalidToken 
    {
        public Guid Id { get; set; }

        public string Jti { get; set; } = null!;

        public string Token { get; set; } = null!;

        public DateTime ExpiryDate { get; set; }

        public DateTime InvalidatedAt { get; set; }
    }
}
