using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.Order
{
    public class InStoreOrderResponseModel
    {
        public required string OrderId { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerPhonenumber { get; set; }
        public string? CustomerEmail { get; set; }

        public required decimal TotalPrice { get; set; }
        public required decimal DiscountAmount { get; set; }
        public required decimal FinalAmount { get; set; }

        public required List<OrderItemResponseModel> Items { get; set; }
        public required EOrderStatus Status { get; set; }

        public required string PaymentId { get; set; }
        public string? PaymentQRCode { get; set; }
        public string? PaymentMethod { get; set; }
        public required string TransactionCode { get; set; }
        public required EPaymentStatus PaymentStatus { get; set; }


        public required string InvoiceId { get; set; }


        public string? Note { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
