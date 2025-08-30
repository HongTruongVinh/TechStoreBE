using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.ShippingDetail;

namespace TechStore.Service.Mappers
{
    public static class ShippingDetailMappers
    {
        public static ShippingDetailResponseModel ToResponseModel(this ShippingDetail shippingDetail, Order order, Shipper shipper)
        {
            return new ShippingDetailResponseModel
            {
                ShippingDetailId = shippingDetail.PublicId,
                OrderId = order.PublicId,
                ShipperId = shipper.PublicId,
                TrackingNumber = shippingDetail.TrackingNumber,
                Status = shippingDetail.Status,
                ShippedDate = shippingDetail.ShippedDate,
                ShippingNote = shippingDetail.ShippingNote,
                EstimatedArrival = shippingDetail.EstimatedArrival,
            };
        }
    }
}
