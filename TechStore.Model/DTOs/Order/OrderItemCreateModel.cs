using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Order
{
    public class OrderItemCreateModel
    {
        public string? OrderId { get; set; }

        public required string ProductVariantOptionId { get; set; }

        public required int Quantity { get; set; }

        public required decimal Discount { get; set; }
    }
}
