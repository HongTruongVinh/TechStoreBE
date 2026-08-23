using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class PaymentSnapshot : BaseEntity
    {
        public required Guid CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public required string ShippingAddress { get; set; }
        public required string CustomerPhoneNumber { get; set; }
        public string? CustomerEmail { get; set; }

        public required decimal SubtotalAmount { get; set; }
        public required decimal ShippingCharge { get; set; }
        public required decimal DiscountAmount { get; set; }
        public required decimal TotalAmount { get; set; }

        public required DateTime ExpiredAt { get; set; }
        public required EPaymentSnapshotStatus Status { get; set; }
        public DateTime? PaidAt { get; set; }

        public string? Note { get; set; }
        public Guid? VoucherId { get; set; }

        public required ICollection<PaymentSnapshotItem> Items { get; set; }
    }
}
