using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class PaymentSnapshot : BaseEntity
    {
        public required Guid CustomerId { get; set; } // public id
        public required string CustomerName { get; set; }
        public required string ShippingAddress { get; set; }
        public required string CustomerPhoneNumber { get; set; }
        public string? CustomerEmail { get; set; }

        public required decimal TotalPrice { get; set; }
        public required decimal ShippingCharge { get; set; }
        public required decimal DiscountAmount { get; set; }
        public required decimal FinalAmount { get; set; }

        public string? Note { get; set; }

        public required ICollection<PaymentSnapshotItem> Items { get; set; }
    }
}
