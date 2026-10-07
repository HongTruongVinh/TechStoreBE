using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities.CategoryEntities;

namespace TechStore.Data.Entities
{
    public class Brand : BaseEntity
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        public required string Slug { get; set; }

        public required string IconImageUrl { get; set; }

        public ICollection<BrandCategory> BrandCategories { get; set; } = [];
    }
}
