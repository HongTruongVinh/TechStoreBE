using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.QrCode;

namespace TechStore.Service.Interfaces
{
    public interface IQRCodeService
    {
        Task<ServiceResult<QRCodeResponseModel>> AddTrackingOrderQRCodeAsync(Order order, string content, string relatedId, EQRCodeType qrCodeType, DateTime? expiredAt);
        Task<ServiceResult<QRCodeResponseModel>> AddPaymentQRCodeAsync(Payment payment, string content, string relatedId, EQRCodeType qrCodeType, DateTime? expiredAt);
        Task<ServiceResult<QRCodeResponseModel>> GetQRCodeAsync(string relatedId, EQRCodeType qrCodeType);
        byte[] GenerateQRCode(string content);
    }
}
