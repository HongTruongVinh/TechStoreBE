using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Voucher
{
    public class VoucherResponseModel
    {
        public required string VoucherId { get; set; }
        public required string Name { get; set; }
        public required decimal DiscountPercent { get; set; }
    }
}
