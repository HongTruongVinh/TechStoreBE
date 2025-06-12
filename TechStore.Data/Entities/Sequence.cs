using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class Sequence
    {
        public string Id { get; set; } = null!; // Tên sequence (vd: "Product", "Order")
        public long Value { get; set; }
    }
}
