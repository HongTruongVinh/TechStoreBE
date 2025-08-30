using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Order
{
    public class CancelOrderModel
    {
        public required string Reason { get; set; }
    }
}
