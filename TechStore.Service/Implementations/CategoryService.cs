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
                    Id = categoryId,
                    Name = model.Name,
                    Description = model.Description,
                    Slug = model.Slug ?? CommonFuntion.GenerateSlug(model.Name),
                    IconImageUrl = model.IconImageUrl ?? CloudinaryFolders.DefaultImage,

                    CreatedAt = DateTime.UtcNow,
                    EntityStatus = EEntityStatus.Active,
                };

                await _uow.Categories.AddAsync(category);

                var result = await _uow.CommitAsync();

                if (result > 0)
                {
                    // Thành công
                }
                else
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

        public Task<ServiceResult<bool>> DeleteCategory(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<List<CategoryResponseModel>>> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<CategoryResponseModel>> GetCategoryById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<bool>> UpdateCategory(string id, CategoryUpdateModel model)
        {
            throw new NotImplementedException();
        }
    }
}
