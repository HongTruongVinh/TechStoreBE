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
        public required Guid OrderId { get; set; }
        public required Order Order { get; set; }

        public required decimal Amount { get; set; } = 0;

        public required EPaymentMethod PaymentMethod { get; set; }

        public required string TransactionCode { get; set; }

        public required EPaymentStatus PaymentStatus { get; set; }


    }
}
