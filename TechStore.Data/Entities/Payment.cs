using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class Payment : BaseEntity
    {
        public required string OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public required decimal Amount { get; set; } = 0;

        public required EPaymentMethod PaymentMethod { get; set; }

        public required string TransactionCode { get; set; }

        public required EPaymentStatus PaymentStatus { get; set; }


    }
}
