using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Brand
{
    public class BrandCreateModel
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        public string? Slug { get; set; }

        public string? IconImageUrl { get; set; }
    }
}
