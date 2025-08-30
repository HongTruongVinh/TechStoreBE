using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Cart
{
    public class CartResponseModel
    {
        public required string CartId { get; set; }

        public required string UserId { get; set; }

        public required decimal TotalPrice { get; set; } = 0;
    }
}
