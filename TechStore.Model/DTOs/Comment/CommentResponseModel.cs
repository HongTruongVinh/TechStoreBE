using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Comment
{
    public class CommentResponseModel
    {
        public required string CommentId { get; set; }
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required string UserAvatar { get; set; }

        public required string ProductId { get; set; }
        public required string Content { get; set; }

        public required int Like { get; set; }
        public required int Dislike { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Stars { get; set; }
    }
}
