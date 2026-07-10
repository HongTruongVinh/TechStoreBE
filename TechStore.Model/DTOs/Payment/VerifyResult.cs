using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Payment
{
    public class VerifyResult
    {
        public required string SnapshotId { get; set; }
        public decimal Amount { get; set; }
        public required string Message { get; set; }
    }
}
