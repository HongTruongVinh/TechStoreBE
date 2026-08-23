using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Order
{
    public class OrderItemCreateModel
    {
        public string? OrderId { get; set; }

        public required string ProductVariantOptionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity phải lớn hơn 0.")]
        public required int Quantity { get; set; }
    }
}
