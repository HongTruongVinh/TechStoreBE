using CloudinaryDotNet.Actions;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Helpers;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.QrCode;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class QRCodeService : IQRCodeService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;

        public QRCodeService(IUnitOfWork uow,
            SequenceGeneratorService sequenceService
            )
        {
            _uow = uow;
            _sequenceService = sequenceService;
        }

        public async Task<ServiceResult<QRCodeResponseModel>> GetQRCodeAsync(string relatedId, EQRCodeType qrCodeType)
        {
            var resultService = new ServiceResult<QRCodeResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.NoExitData,
            };

            var qrCode = await _uow.QRCodes.FindOneAsync(qr => qr.RelatedPublicId == relatedId);

            if (qrCode != null)
            {
                resultService.IsSuccess = true;
                resultService.Data = new QRCodeResponseModel
                {
                    Id = qrCode.PublicId,
                    RelatedId = relatedId,
                    Content = qrCode.Content,
                    Type = qrCode.Type,
                    ImageData = qrCode.ImageData,
                    CreatedAt = qrCode.CreatedAt,
                    ExpiredAt = qrCode.ExpiredAt,
                };
                resultService.Message = Messenger.GetDataSuccessful;
            }

            return resultService;
        }

        public byte[] GenerateQRCode(string content)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new BitmapByteQRCode(qrData);
            return qrCode.GetGraphic(20);
        }

        public async Task<ServiceResult<QRCodeResponseModel>> AddTrackingOrderQRCodeAsync(
            Order order,
            string content, 
            string relatedId, 
            EQRCodeType qrCodeType, 
            DateTime? expiredAt
            )
        {
            var serviceResult = new ServiceResult<QRCodeResponseModel>
            {
                IsSuccess = true,
                Data = null,
                Message = string.Empty,
            };

            var publicId = await _uow.QRCodes.CountAsync();
            QRCode qrCode = new QRCode
            {
                Content = content,
                PublicId = publicId.ToString(),
                RelatedPublicId = relatedId,
                RelatedId = order.Id,
                ImageData = GenerateQRCode(content),
                Type = qrCodeType,
                ExpiredAt = expiredAt,
                EntityStatus = EEntityStatus.Active,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
            };

            await _uow.QRCodes.AddAsync(qrCode);
            serviceResult.Data = qrCode.ToQRCodeResponseModel();

            //var result = await _uow.CommitAsync();

            //if (result < 1)
            //{
            //    serviceResult.Message = Messenger.CreateDataError;
            //    return serviceResult;
            //}

            //var resultAddQRCode = await _uow.QRCodes.FindOneAsync(qr => qr.PublicId == qrCode.PublicId);

            //if (resultAddQRCode != null)
            //{
            //    serviceResult.IsSuccess = true;
            //    serviceResult.Data = new QRCodeResponseModel
            //    {
            //        Id = resultAddQRCode.PublicId,
            //        Content = resultAddQRCode.Content,
            //        ImageData = resultAddQRCode.ImageData,
            //        Type = resultAddQRCode.Type,
            //        RelatedId = resultAddQRCode.RelatedPublicId,
            //        CreatedAt = resultAddQRCode.CreatedAt,
            //        ExpiredAt = resultAddQRCode.ExpiredAt,
            //    };
            //}

            return serviceResult;
        }

        public async Task<ServiceResult<QRCodeResponseModel>> AddPaymentQRCodeAsync(
            Payment payment,
            string content,
            string relatedId,
            EQRCodeType qrCodeType,
            DateTime? expiredAt
            )
        {
            var serviceResult = new ServiceResult<QRCodeResponseModel>
            {
                IsSuccess = true,
                Data = null,
                Message = string.Empty,
            };

            var publicId = await _uow.QRCodes.CountAsync();
            QRCode qrCode = new QRCode
            {
                Content = content,
                PublicId = publicId.ToString(),
                RelatedPublicId = relatedId,
                RelatedId = payment.Id,
                ImageData = GenerateQRCode(content),
                Type = qrCodeType,
                ExpiredAt = expiredAt,
                EntityStatus = EEntityStatus.Active,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
            };

            await _uow.QRCodes.AddAsync(qrCode);
            serviceResult.Data = qrCode.ToQRCodeResponseModel();

            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                serviceResult.Message = Messenger.CreateDataError;
                return serviceResult;
            }

            //var resultAddQRCode = await _uow.QRCodes.FindOneAsync(qr => qr.PublicId == qrCode.PublicId);

            //if (resultAddQRCode != null)
            //{
            //    serviceResult.IsSuccess = true;
            //    serviceResult.Data = new QRCodeResponseModel
            //    {
            //        Id = resultAddQRCode.PublicId,
            //        Content = resultAddQRCode.Content,
            //        ImageData = resultAddQRCode.ImageData,
            //        Type = resultAddQRCode.Type,
            //        RelatedId = resultAddQRCode.RelatedPublicId,
            //        CreatedAt = resultAddQRCode.CreatedAt,
            //        ExpiredAt = resultAddQRCode.ExpiredAt,
            //    };
            //}

            return serviceResult;
        }
    }
}
