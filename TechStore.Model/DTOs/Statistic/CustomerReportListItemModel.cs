using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Statistic
{
    public class CustomerReportListItemModel
    {
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
        public required int OrderCount { get; set; }

    }
}
