using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class Order : BaseEntity
    {
        public required Guid CustomerId { get; set; }
        public required string CustomerPublicId { get; set; }
        public User Customer { get; set; } = null!;

        public required decimal SubtotalAmount { get; set; }
        public required decimal ShippingCharge { get; set; }
        public required decimal DiscountAmount { get; set; }
        public required decimal TotalAmount { get; set; }

        public required string CustomerName { get; set; }
        public string? ShippingAddress { get; set; }
        public required string CustomerPhoneNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public string? Note { get; set; }

        public required EOrderStatus OrderStatus { get; set; } = EOrderStatus.Pending;
        public Invoice? Invoice { get; set; }
        public ShippingDetail? ShippingDetail { get; set; }
        public required ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
