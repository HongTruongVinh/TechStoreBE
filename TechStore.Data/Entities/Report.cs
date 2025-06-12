using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities
{
    public class Report : BaseEntity
    {
        public required string Title { get; set; }

        public required string Data { get; set; }
    }
}
