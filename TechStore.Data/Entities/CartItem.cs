using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class CartItem : BaseEntity
    {
        public required string UserId { get; set; }
        public User User { get; set; } = null!;

        public required string ProductId { get; set; }

        public required int Quantity { get; set; }

        public required decimal Discount { get; set; }

        public required decimal TotalPrice { get; set; } = 0;
    }
}
