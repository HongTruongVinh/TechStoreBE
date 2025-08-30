using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.QrCode
{
    public class QRCodeResponseModel
    {
        public required string Id { get; set; } // GUID hoặc ObjectId

        public required string Content { get; set; } // Ví dụ: link đến Momo, VNPay, order ID, v.v.

        public required byte[] ImageData { get; set; } // Mã QR dưới dạng ảnh (byte[])

        public required EQRCodeType Type { get; set; } // "Payment", "OrderTracking", etc.

        public required string RelatedId { get; set; } // Id của Payment hoặc Order

        public required DateTime CreatedAt { get; set; }

        public DateTime? ExpiredAt { get; set; }
    }
}
