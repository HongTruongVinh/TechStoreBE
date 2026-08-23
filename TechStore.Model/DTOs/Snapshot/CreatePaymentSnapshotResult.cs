using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.Snapshot
{
    public class CreatePaymentSnapshotResult
    {
        public required string SnapshotId { get; set; } 
        public string? OrderId { get; set; } 
        public required decimal Amount { get; set; } 
        public required EPaymentSnapshotStatus Status { get; set; } 
        public string? Message { get; set; } 
    }
}
