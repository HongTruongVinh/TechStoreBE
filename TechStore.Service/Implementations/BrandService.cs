using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.CommonFunction;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Helpers;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Brand;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;

        public BrandService(IUnitOfWork uow, SequenceGeneratorService sequenceService)
        {
            _uow = uow;
            _sequenceService = sequenceService;
        }

        public async Task<ServiceResult<string>> AddBrand(BrandCreateModel model)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.CreateDataError
            };

            string publicId = await _sequenceService.GetNextBrandIdAsync();

            var brand = new Brand
            {
                PublicId = publicId,
                Name = model.Name,
                Description = model.Description,
                Slug = model.Slug ?? CommonFuntion.GenerateSlug(model.Name),
                IconImageUrl = model.IconImageUrl ?? CloudinaryFolders.DefaultImage,

                EntityStatus = EEntityStatus.Active,
                CreatedAt = TimeZoneHelper.GetUtcNow(),
            };

            await _uow.Brands.AddAsync(brand);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = publicId;
            serviceResult.Message = Messenger.SuccessFull;
            return serviceResult;
        }

        public async Task<ServiceResult<bool>> DeleteBrand(string id)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NoExitData
            };

            var brand = await _uow.Brands.GetByIdAsync(id);

            if (brand == null)
            {
                return serviceResult;
            }

            brand.EntityStatus = EEntityStatus.Deleted;

            _uow.Brands.Update(brand);
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

        public async Task<ServiceResult<List<BrandResponseModel>>> GetAllBrands()
        {
            var serviceResult = new ServiceResult<List<BrandResponseModel>>
            {
                IsSuccess = true,
                Data = new List<BrandResponseModel>(),
                Message = Messenger.GetDataSuccessful
            };

            var brands = await _uow.Brands.GetAllAsync();

            if (brands == null || !brands.Any())
            {
                serviceResult.IsSuccess = false;
                serviceResult.Message = Messenger.NoExitData;
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;
            serviceResult.Data = brands.ToList().ToListBrandResponseModels();

            return serviceResult;
        }

        public async Task<ServiceResult<BrandResponseModel>> GetBrandById(string id)
        {
            var serviceResult = new ServiceResult<BrandResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.NoExitData
            };

            var brand = await _uow.Brands.GetByIdAsync(id);

            if (brand == null)
            {
                return serviceResult;
            }


            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;
            serviceResult.Data = brand.ToBrandResponseModel();

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateBrand(string id, BrandUpdateModel model)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NoExitData
            };

            var brand = await _uow.Brands.GetByIdAsync(id);

            if (brand == null)
            {
                return serviceResult;
            }

            brand.Name = string.IsNullOrEmpty(model.Name) ? brand.Name : model.Name;
            brand.Description = string.IsNullOrEmpty(model.Description) ? brand.Description : model.Description;
            brand.Slug = string.IsNullOrEmpty(model.Slug) ? brand.Slug : model.Slug;
            brand.IconImageUrl = string.IsNullOrEmpty(model.IconImageUrl) ? brand.IconImageUrl : model.IconImageUrl;

            _uow.Brands.Update(brand);
            var result = await _uow.CommitAsync();

            if (result < 0)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.Data = true;
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;

            return serviceResult;
        }
    }
}
