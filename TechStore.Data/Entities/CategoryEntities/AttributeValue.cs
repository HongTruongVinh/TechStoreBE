using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities.CategoryEntities
{
    public class AttributeValue : BaseEntity
    {
        public Guid AttributeId { get; set; }
        public required string Value { get; set; }
        public required string DisplayValue { get; set; }
    }
}
