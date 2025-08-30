using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class Invoice: BaseEntity
    {
        public required Guid OrderId { get; set; }
        public required Order Order { get; set; }

        public required decimal TotalPrice { get; set; } = 0;

        public required decimal DiscountAmount { get; set; }

        public required decimal FinalAmount { get; set; }

        public DateTime? PaidAt { get; set; }

        public string? CashierName { get; set; }

        public required EInvoiceStatus InvoiceStatus { get; set; } = EInvoiceStatus.Unpaid;

    }
}
