using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;
using TechStore.Model.DTOs.QrCode;

namespace TechStore.Model.DTOs.Payment
{
    public class PaymentDataForSnapshotModel
    {
        public required string SnapshotId { get; set; }

        public required decimal Amount { get; set; }
        public required string QrDataURL { get; set; }

        public required DateTime CreatedAt { get; set; }
        public required DateTime ExpiredAt { get; set; }
    }

    public class PaymentDataModel
    {
        public required string PaymentId { get; set; }

        public required decimal Amount { get; set; }
        public required string QrDataURL { get; set; }

        public required DateTime CreatedAt { get; set; }
        public required DateTime ExpiredAt { get; set; }
    }
}
