using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Order
{
    public class PrepayOrderResult
    {
        public required string OrderId { get; set; }
        public required string PaymentId { get; set; }
    }
}
