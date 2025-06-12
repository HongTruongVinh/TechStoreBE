using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class Comment : BaseEntity
    {
        public required string UserId { get; set; }
        public User? User { get; set; }

        public required string ProductId { get; set; }
        public Product? Product { get; set; }

        public string? ParentCommentId { get; set; }

        public required string Content { get; set; }

        public required int Like { get; set; }

        public required int Dislike { get; set; }

        public int? Stars { get; set; } // 1 - 5

        public required bool IsVerifiedPurchase { get; set; }

        public required bool IsApproved { get; set; }
    }
}
