using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;

namespace TechStore.Data.Entities
{
    public class ShippingDetail : BaseEntity
    {
        public required Guid OrderId { get; set; }
        public required Order Order { get; set; }
        public required string OrderPublicId { get; set; }

        public required Guid ShipperId { get; set; }
        public required Shipper Shipper { get; set; }
        public required string ShipperPublicId { get; set; }

        public required string ShipperName { get; set; }
        public string? DeliveryStaffName { get; set; }
        public string? DeliveryStaffPhoneNumber { get; set; }

        public required string TrackingNumber { get; set; }
        public EShippingStatus Status { get; set; }    // Trạng thái: Preparing, Shipping, Delivered, Failed...

        public DateTime? ShippedDate { get; set; }
        public DateTime? EstimatedArrival { get; set; }
        public string? ShippingNote { get; set; }
        public int FailureCount { get; set; } = 0; // Số lần giao hàng thất bại
    }
}
