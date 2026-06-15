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
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public required decimal TotalAmount { get; set; }
        public required decimal PaidAmount { get; set; }
        public decimal RemainingAmount => TotalAmount - PaidAmount;
        public DateTime? PaidAt { get; set; }
        public required EInvoiceStatus InvoiceStatus { get; set; } = EInvoiceStatus.Unpaid;
        public required ICollection<Payment> Payments { get; set; }

        public Guid? CashierId { get; set; }

    }
}
