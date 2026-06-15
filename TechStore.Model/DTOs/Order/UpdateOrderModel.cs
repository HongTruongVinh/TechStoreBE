using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Order
{
    public class UpdateOrderModel
    {
        public required string CustomerName { get; set; }
        public required string CustomerPhoneNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public required string ShippingAddress { get; set; }
        public string? Note { get; set; }
    }
}
