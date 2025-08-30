using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Comment
{
    public class CommentCreateModel
    {
        public string? ParentCommentId { get; set; }

        public required string ProductId { get; set; }

        public required string Content { get; set; }

        public int? Stars { get; set; } // 1 - 5
    }
}
