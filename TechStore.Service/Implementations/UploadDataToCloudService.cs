using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Model.DTOs.ResponseModel;
using TechStore.Service.Interfaces;

namespace TechStore.Service.Implementations
{
    public class UploadDataToCloudService : IUploadDataToCloudService
    {
        private Cloudinary _cloudinary;
        private readonly IOptions<CloudinaryConfig> _cloudinaryConfig;

        public UploadDataToCloudService(IOptions<CloudinaryConfig> cloudinaryConfig)
        {
            var accountCloud = new Account(cloudinaryConfig.Value.CloudName, cloudinaryConfig.Value.ApiKey, cloudinaryConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(accountCloud);

            _cloudinaryConfig = cloudinaryConfig;
        }

        public async Task<UploadWithCloudinaryResponseModel> AddPhotoToCloudAsync(PhotoUploadModel photoUploadModel)
        {
            var uploadResult = new ImageUploadResult();

            if (photoUploadModel.formFile.Length > 0)
            {
                using var stream = photoUploadModel.formFile.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(photoUploadModel.formFile.FileName, stream),
                    Transformation = new Transformation().Height(500).Crop("fill").Quality("auto")
                };

                if (photoUploadModel.PhotoType == EPhotoType.Brand)
                {
                    uploadParams.Folder = CloudinaryFolders.Brands;
                }
                else if (photoUploadModel.PhotoType == EPhotoType.Category)
                {
                    uploadParams.Folder = CloudinaryFolders.Categories;
                }
                else if (photoUploadModel.PhotoType == EPhotoType.Product)
                {
                    uploadParams.Folder = CloudinaryFolders.Products;
                }

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            var resultData = new UploadWithCloudinaryResponseModel()
            {
                UrlPicture = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };

            return resultData;
        }

        public async Task<UploadWithCloudinaryResponseModel> AddPhotoToCloudAsyncByBase64(string imagesBase64)
        {
            var uploadResult = new ImageUploadResult();
            string dataUpload = $"data:image/jpeg;base64,{imagesBase64}";
            if (imagesBase64.Length > 0)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(@$"{dataUpload}"),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Format = "png"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            var resultData = new UploadWithCloudinaryResponseModel()
            {
                UrlPicture = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };

            return resultData;
        }

        public async Task<DeletionResult> DeletePhotoToCloudAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }

        public async Task DeleteFolderAsync(string folderName)
        {
            var result = await _cloudinary.ListResourcesByPrefixAsync(folderName);

            if (result.Resources.Length > 0)
            {
                var publicIds = result.Resources.Select(r => r.PublicId).ToList();

                foreach (var publicId in publicIds)
                {
                    await DeletePhotoToCloudAsync(publicId);
                }
            }
        }

    }
}
