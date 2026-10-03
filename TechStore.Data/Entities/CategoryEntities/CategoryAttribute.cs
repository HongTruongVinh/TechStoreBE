using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities.CategoryEntities
{
    public class CategoryAttribute : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public Guid AttributeId { get; set; }
        public required bool IsFilterable { get; set; }
        public required bool IsRequired { get; set; }
        public required string DisplayOrder { get; set; }
    }
}
