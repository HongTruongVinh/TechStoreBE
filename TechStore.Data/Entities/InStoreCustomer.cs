using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class InStoreCustomer : BaseEntity
    {
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
