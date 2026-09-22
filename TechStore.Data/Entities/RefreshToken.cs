using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }// Internal ID (Primary Key)

        public required Guid UserId { get; set; }

        public required string TokenHash { get; set; }

        public required DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string? ReplacedByTokenHash { get; set; }

        public string? CreatedByIp { get; set; }

        public required DateTime CreatedAt { get; set; }

        public User User { get; set; } = null!;
    }
}
