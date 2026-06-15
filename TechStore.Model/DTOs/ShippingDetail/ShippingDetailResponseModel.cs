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
        public required string Id { get; set; }
        public required string OrderId { get; set; }        
        public required string ShipperId { get; set; }

        public required string ShipperName { get; set; }
        public string? DeliveryStaffName { get; set; }
        public string? DeliveryStaffPhoneNumber { get; set; }

        public required string TrackingNumber { get; set; }    // Mã vận đơn
        public required EShippingStatus Status { get; set; }   

        public DateTime? ShippedDate { get; set; }
        public DateTime? EstimatedArrival { get; set; }
        public string? ShippingNote { get; set; }
    }
}
