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
        public static ShippingDetailResponseModel ToResponseModel(this ShippingDetail shippingDetail)
        {
            return new ShippingDetailResponseModel
            {
                Id = shippingDetail.PublicId,
                OrderId = shippingDetail.OrderPublicId,
                ShipperId = shippingDetail.ShipperPublicId,
                ShipperName = shippingDetail.ShipperName,
                TrackingNumber = shippingDetail.TrackingNumber,
                Status = shippingDetail.Status,
                ShippedDate = shippingDetail.ShippedDate,
                ShippingNote = shippingDetail.ShippingNote,
                EstimatedArrival = shippingDetail.EstimatedArrival,
            };
        }
    }
}
