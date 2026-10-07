using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Model.DTOs.Brand;

namespace TechStore.Model.DTOs.Category
{
    public class CategoryResponseModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public required string Slug { get; set; }

        public required string IconImageUrl { get; set; }

        public List<BrandResponseModel> Brands { get; set; } = [];
    }
}
