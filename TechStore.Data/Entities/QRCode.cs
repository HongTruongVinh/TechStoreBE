using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class QRCode : BaseEntity
    {
        public required string Content { get; set; } // Ví dụ: link đến Momo, VNPay, order ID, v.v.

        public required byte[] ImageData { get; set; } // Mã QR dưới dạng ảnh (byte[])

        public required EQRCodeType Type { get; set; } // "Payment", "OrderTracking", etc.

        public required Guid RelatedId { get; set; } // Id của Payment hoặc Order
        public required string RelatedPublicId { get; set; } // Id của Payment hoặc Order

        public DateTime? ExpiredAt { get; set; }
    }
}
