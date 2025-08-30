using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Enums;
using TechStore.Model.DTOs.ResponseModel;

namespace TechStore.Service.Interfaces
{
    public interface IUploadDataToCloudService
    {
        Task<UploadWithCloudinaryResponseModel> AddPhotoToCloudAsync(PhotoUploadModel photoCreateModel);

        Task<UploadWithCloudinaryResponseModel> AddPhotoToCloudAsyncByBase64(string imagesBase64);

        Task<DeletionResult> DeletePhotoToCloudAsync(string publicId);

        Task DeleteFolderAsync(string folderName);
    }


    public class PhotoUploadModel
    {
        public required IFormFile formFile { get; set; }

        public required EPhotoType PhotoType { get; set; }
    }
}
