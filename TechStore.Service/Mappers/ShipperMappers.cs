using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Shipper;

namespace TechStore.Service.Mappers
{
    public static class ShipperMappers
    {
        public static ShipperResponseModel ToResponseModel(this Shipper shipper)
        {
            return new ShipperResponseModel
            {
                ShipperId = shipper.PublicId,
                Name = shipper.Name,
                Website = shipper.Website,
                Description = shipper.Description,
                SupportPhone = shipper.SupportPhone,
                LogoUrl = shipper.LogoUrl,
                IsActive = shipper.IsActive,
                CreatedAt = shipper.CreatedAt
            };
        }

        public static List<ShipperResponseModel> ToListResponseModels(this List<Shipper> shippers)
        {
            var models = new List<ShipperResponseModel>();

            foreach (var shipper in shippers)
            {
                models.Add(ToResponseModel(shipper));
            }

            return models;
        }
    }
}
