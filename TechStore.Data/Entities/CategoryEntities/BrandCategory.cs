using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities.CategoryEntities
{
    public class BrandCategory
    {
        public required Guid BrandId { get; set; }
        public required Guid CategoryId { get; set; }

        public virtual Brand Brand { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }
}
