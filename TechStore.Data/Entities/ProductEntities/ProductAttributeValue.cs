using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities.ProductEntities
{
    public class ProductAttributeValue : BaseEntity
    {
        public Guid ProductVariantId { get; set; }
        public Guid AttributeValueId { get; set; }
        public Guid AttributeId { get; set; }
    }
}
