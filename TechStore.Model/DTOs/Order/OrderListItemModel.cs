using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.Order
{
    public class OrderListItemModel
    {
        public required string OrderId { get; set; }
        public required string CustomerId { get; set; }
        public required string CustomerName { get; set; }

        public required string CustomerPhonenumber { get; set; }
        public required string CustomerEmail { get; set; }
        public required EOrderStatus OrderStatus { get; set; } = EOrderStatus.Pending;

        public required string ShippingAddress { get; set; }
        public required decimal TotalPrice { get; set; }
        public required decimal DiscountAmount { get; set; }

        public required decimal ShippingCharge { get; set; }
        public required decimal FinalAmount { get; set; }
        public required EPaymentMethod PaymentMethod { get; set; }

        public required EOrderType OrderType { get; set; }

        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
