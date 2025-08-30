using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.CommonFunction;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Category;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;

        public CategoryService(IUnitOfWork uow,
            SequenceGeneratorService sequenceService
            )
        {
            _uow = uow;
            _sequenceService = sequenceService;
        }

        public async Task<ServiceResult<string>> AddCategory(CategoryCreateModel model)
        {
            ServiceResult<string> serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.CreateDataError
            };

            try
            {
                var categoryId = await _sequenceService.GetNextCategoryIdAsync();
                var category = new Category
                {
                    PublicId = categoryId,
                    Name = model.Name,
                    Description = model.Description,
                    Slug = model.Slug ?? CommonFuntion.GenerateSlug(model.Name),
                    IconImageUrl = model.IconImageUrl ?? CloudinaryFolders.DefaultImage,

                    CreatedAt = DateTime.UtcNow,
                    EntityStatus = EEntityStatus.Active,
                };

                await _uow.Categories.AddAsync(category);

                var result = await _uow.CommitAsync();

                if (result < 1)
                {
                    return serviceResult;
                }

                serviceResult.IsSuccess = true;
                serviceResult.Data = categoryId;
                serviceResult.Message = Messenger.SuccessFull;

                return serviceResult;
            }
            catch
            {
                return serviceResult;
            }
        }

        public async Task<ServiceResult<List<CategoryResponseModel>>> GetAllCategories()
        {
            ServiceResult<List<CategoryResponseModel>> serviceResult = new ServiceResult<List<CategoryResponseModel>>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.NoExitData
            };

            var categories = await _uow.Categories.GetAllAsync();

            if (categories == null)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;
            serviceResult.Data = categories.ToList().ToListCategoryResponseModels();

            return serviceResult;
        }

        public async Task<ServiceResult<CategoryResponseModel>> GetCategoryById(string id)
        {
            ServiceResult<CategoryResponseModel> serviceResult = new ServiceResult<CategoryResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.NoExitData
            };

            var category = await _uow.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return serviceResult;
            }

            serviceResult.Data = category.ToCategoryResponseModel();
            serviceResult.IsSuccess = true;
            serviceResult.Message= Messenger.GetDataSuccessful;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateCategory(string categoryId, CategoryUpdateModel categoryModel)
        {
            ServiceResult<bool> serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NoExitData
            };

            var category = await _uow.Categories.GetByIdAsync(categoryId);

            if (category == null)
            {
                return serviceResult;
            }

            category.Name = string.IsNullOrEmpty(categoryModel.Name) ? category.Name : categoryModel.Name;
            category.Description = string.IsNullOrEmpty(categoryModel.Description) ? category.Description : categoryModel.Description;
            category.Slug = string.IsNullOrEmpty(categoryModel.Slug) ? category.Slug : categoryModel.Slug;
            category.IconImageUrl = string.IsNullOrEmpty(categoryModel.IconImageUrl) ? category.IconImageUrl : categoryModel.IconImageUrl;

            _uow.Categories.Update(category);
            var result = await _uow.CommitAsync();
            if (result < 1)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> DeleteCategory(string id)
        {
            ServiceResult<bool> serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NoExitData
            };

            var category = await _uow.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return serviceResult;
            }

            category.EntityStatus = EEntityStatus.Deleted;

            _uow.Categories.Update(category);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.Data = true;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
        }
    }
}
