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
        public string? CustomerId { get; set; }
        public User? Customer { get; set; }

        public required EOrderType OrderType { get; set; } = EOrderType.Online;

        public required EOrderStatus OrderStatus { get; set; } = EOrderStatus.Pending;

        public required decimal TotalPrice { get; set; }

        public required decimal ShippingCharge { get; set; } = 0;

        public required decimal DiscountAmount { get; set; }

        public required decimal FinalAmount { get; set; }

        public required string CustomerName { get; set; }

        public string? ShippingAddress { get; set; }

        public required string CustomerPhoneNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public required EPaymentMethod PaymentMethod { get; set; }
        public string? Note { get; set; }

        public Invoice? Invoice { get; set; }
        public Payment? Payment { get; set; }
        public QRCode? QRCode { get; set; }
        public ShippingDetail? ShippingDetail { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
