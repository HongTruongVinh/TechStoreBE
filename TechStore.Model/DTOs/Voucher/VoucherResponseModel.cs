using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.Voucher
{
    public class VoucherResponseModel
    {
        public required string Id { get; set; }
        public required string Code { get; set; }
        public required string Description { get; set; }

        public required EDiscountType DiscountType { get; set; }
        public required decimal DiscountValue { get; set; }
        public required decimal MaxDiscountAmount { get; set; }
        public required decimal MinOrderPrice { get; set; }

        public required int UsageLimit { get; set; }
        public decimal Available { get; set; }

        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required EVoucherStatus Status { get; set; }
    }
}
