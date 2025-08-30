using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.Order
{
    public class InStoreOrderCreateModel
    {
        public required string CustomerName { get; set; }
        public required string CustomerPhonenumber { get; set; }
        public string? CustomerEmail { get; set; }
        public string? VoucherCode { get; set; }
        public required List<OrderItemCreateModel> Items { get; set; }

        public required EPaymentMethod PaymentMethod { get; set; }
    }
}
