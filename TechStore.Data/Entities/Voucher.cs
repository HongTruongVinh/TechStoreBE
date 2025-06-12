using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class Voucher : BaseEntity
    {
        public required string Title { get; set; }

        public required decimal DiscountPercent { get; set; }
    }
}
