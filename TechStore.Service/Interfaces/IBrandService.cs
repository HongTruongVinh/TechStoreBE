using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Brand;

namespace TechStore.Service.Interfaces
{
    public interface IBrandService
    {
        Task<ServiceResult<string>> AddBrand(BrandCreateModel model);
        Task<ServiceResult<bool>> UpdateBrand(string id, BrandUpdateModel model);
        Task<ServiceResult<bool>> DeleteBrand(string id);
        Task<ServiceResult<BrandResponseModel>> GetBrandById(string id);
        Task<ServiceResult<List<BrandResponseModel>>> GetAllBrands();
    }
}
