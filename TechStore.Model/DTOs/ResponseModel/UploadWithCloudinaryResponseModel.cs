using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.ResponseModel
{
    public class UploadWithCloudinaryResponseModel
    {
        public string? UrlPicture { get; set; }

        public string? PublicId { get; set; }
    }
}
