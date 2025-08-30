using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Comment
{
    public class CommentUpdateModel
    {
        public required string Content { get; set; }

        public int? Stars { get; set; } // 1 - 5

    }
}
