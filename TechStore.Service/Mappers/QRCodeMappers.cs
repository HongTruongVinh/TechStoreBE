using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.QrCode;

namespace TechStore.Service.Mappers
{
    public static class QRCodeMappers
    {
        public static QRCodeResponseModel ToQRCodeResponseModel(this QRCode qrCode)
        {
            return new QRCodeResponseModel
            {
                Id = qrCode.PublicId,
                RelatedId = qrCode.RelatedPublicId,
                Content = qrCode.Content,
                ImageData = qrCode.ImageData,
                Type = qrCode.Type,
                ExpiredAt = qrCode.ExpiredAt,
                CreatedAt = qrCode.CreatedAt
            };
        }
    }
}
