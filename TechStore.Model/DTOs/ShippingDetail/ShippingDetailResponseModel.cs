using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Model.DTOs.ShippingDetail
{
    public class ShippingDetailResponseModel
    {
        public required string ShippingDetailId { get; set; }

        public required string OrderId { get; set; }         // Liên kết với Order

        public required string ShipperId { get; set; }       // Đơn vị vận chuyển

        public required string TrackingNumber { get; set; }    // Mã vận đơn

        public required EShippingDetailStatus Status { get; set; }    // Trạng thái: Preparing, Shipping, Delivered, Failed...

        public DateTime? ShippedDate { get; set; }

        public DateTime? EstimatedArrival { get; set; }

        public string? ShippingNote { get; set; }
    }
}
