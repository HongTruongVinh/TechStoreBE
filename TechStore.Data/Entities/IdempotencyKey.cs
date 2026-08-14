using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class IdempotencyKey : BaseEntity
    {
        public required Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public required string RequestKey { get; set; }

        public required string Endpoint { get; set; }
        public required string RequestHash { get; set; }
        public required int StatusCode { get; set; }
        public required string ResponseBody { get; set; }

        public required DateTime ExpiredAt { get; set; }
    }
}
