using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Category;
using TechStore.Service.Implementations;

namespace TechStore.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceResult<string>> AddCategory(CategoryCreateModel model);
        Task<ServiceResult<bool>> UpdateCategory(string id, CategoryUpdateModel model);
        Task<ServiceResult<bool>> DeleteCategory(string id);
        Task<ServiceResult<CategoryResponseModel>> GetCategoryById(string id);
        Task<ServiceResult<List<CategoryResponseModel>>> GetAllCategories();
    }

}
