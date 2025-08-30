using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Model.DTOs.ResponseModel;
using TechStore.Service.Interfaces;

namespace TechStoreAPI.Controllers
{

    [Route(RouterControllerName.UploadToCloud)]
    [ApiController]
    public class UploadToCloudController : ControllerBase
    {
        private readonly IUploadDataToCloudService _uploadDataToCloudService;

        public UploadToCloudController(IUploadDataToCloudService uploadDataToCloudService)
        {
            _uploadDataToCloudService = uploadDataToCloudService;
        }

        [HttpPost("cloudinary")]
        public async Task<ApiResponse<UploadWithCloudinaryResponseModel>> AddPhotoToCloudinaryAsync(PhotoUploadModel model)
        {
            ApiResponse<UploadWithCloudinaryResponseModel> result = new()
            {
                PartnerCode = Messenger.SuccessFull,
                RetCode = ERetCode.Successfull,
                Data = new UploadWithCloudinaryResponseModel(),
                SystemMessage = string.Empty,
                StatusCode = (int)HttpStatusCode.Created
            };

            result.Data = await _uploadDataToCloudService.AddPhotoToCloudAsync(model);
            return result;
        }

        [HttpPost("cloudinary-request")]
        public async Task<ApiResponse<UploadWithCloudinaryResponseModel>> AddPhotoToCloudinaryRequestAsync()
        {
            ApiResponse<UploadWithCloudinaryResponseModel> result = new()
            {
                PartnerCode = Messenger.SuccessFull,
                RetCode = ERetCode.Successfull,
                Data = new UploadWithCloudinaryResponseModel(),
                SystemMessage = string.Empty,
                StatusCode = (int)HttpStatusCode.Created
            };
            var files = Request.Form.Files;

            var fileTypeValue = Request.Form["photoType"];
            Enum.TryParse<EPhotoType>(fileTypeValue, true, out var photoType);

            var uploadPhotoModel = new PhotoUploadModel
            {
                formFile = files[0],
                PhotoType = photoType
            };

            result.Data = await _uploadDataToCloudService.AddPhotoToCloudAsync(uploadPhotoModel);
            return result;
        }

        [HttpDelete("cloudinary/{**photoPublicId}")]
        public async Task<ApiResponse<DeletionResult>> DeletePhotoToCloudinarAsync([FromRoute] string photoPublicId)
        {
            if (photoPublicId.Contains(CloudinaryFolders.DefaultImage))
            {
                return new ApiResponse<DeletionResult>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = new DeletionResult(),
                    SystemMessage = string.Empty,
                    StatusCode = (int)HttpStatusCode.Created
                };
            }

            ApiResponse<DeletionResult> result = new()
            {
                PartnerCode = Messenger.SuccessFull,
                RetCode = ERetCode.Successfull,
                Data = new DeletionResult(),
                SystemMessage = string.Empty,
                StatusCode = (int)HttpStatusCode.Created
            };

            result.Data = await _uploadDataToCloudService.DeletePhotoToCloudAsync(photoPublicId);
            return result;
        }
    }
}
