using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Order
{
    public class UpdateOrderToDeliveringModel
    { 
        public required string ShipperId { get; set; }
    }
}
