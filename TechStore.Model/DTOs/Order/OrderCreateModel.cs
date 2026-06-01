using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;
using TechStoreModel.Model.ResponseModel;

namespace TechStore.Model.DTOs.Order
{
    public class OrderCreateModel
    {
        public required string CustomerName { get; set; }
        public required string CustomerPhoneNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public required string ShippingAddress { get; set; }
        public string? VoucherCode { get; set; }
        public string? Note { get; set; }
        public required List<OrderItemCreateModel> Items { get; set; }

        public required EPaymentMethod PaymentMethod { get; set; }
    }
}
