using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Voucher
{
    public class VoucherCreateModel
    {
        public required string Name { get; set; }
        public required decimal DiscountPercent { get; set; }
    }
}
