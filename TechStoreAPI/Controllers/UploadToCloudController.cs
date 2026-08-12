using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Model.DTOs.ResponseModel;
using TechStore.Common.Models;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

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

        [HttpPost("cloudinary-request")]
        public async Task<ActionResult<ApiResponse<UploadWithCloudinaryResponseModel>>> AddPhotoToCloudinaryRequestAsync()
        {
            var files = Request.Form.Files;
            var fileTypeValue = Request.Form["photoType"];
            Enum.TryParse(fileTypeValue, true, out EPhotoType photoType);

            var uploadPhotoModel = new PhotoUploadModel
            {
                formFile = files[0],
                PhotoType = photoType
            };

            var data = await _uploadDataToCloudService.AddPhotoToCloudAsync(uploadPhotoModel);

            return ServiceResult<UploadWithCloudinaryResponseModel>.Success(data).ToActionResult(this);
        }

        [HttpDelete("cloudinary/{**photoPublicId}")]
        public async Task<ActionResult<ApiResponse<DeletionResult>>> DeletePhotoToCloudinarAsync([FromRoute] string photoPublicId)
        {
            if (photoPublicId.Contains(CloudinaryFolders.DefaultImage))
            {
                return ServiceResult<DeletionResult>.Success(new DeletionResult()).ToActionResult(this);
            }

            var data = await _uploadDataToCloudService.DeletePhotoToCloudAsync(photoPublicId);

            return ServiceResult<DeletionResult>.Success(data).ToActionResult(this);
        }
    }
}
