using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Category
{
    public class CategoryUpdateModel
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        public required string Slug { get; set; }

        public string? IconImageUrl { get; set; }
    }
}
