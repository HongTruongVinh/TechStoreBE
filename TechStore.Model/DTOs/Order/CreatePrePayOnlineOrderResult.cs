using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Order
{
    public class CreatePrePayOnlineOrderResult
    {
        public required string OrderId { get; set; }
        public required string PaymentSnapshotId { get; set; }
        public required decimal Amount { get; set; }
    }
}
