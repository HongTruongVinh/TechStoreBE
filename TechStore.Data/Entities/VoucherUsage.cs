using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class VoucherUsage : BaseEntity
    {
        public required Guid VoucherId { get; set; }
        public Voucher Voucher { get; set; } = null!;

        public required Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public required Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public required DateTime UsedAt { get; set; }
    }
}
