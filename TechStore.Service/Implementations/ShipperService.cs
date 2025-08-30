
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Shipper;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class ShipperService : IShipperService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;

        public ShipperService(IUnitOfWork uow, SequenceGeneratorService sequenceService)
        {
            _uow = uow;
            _sequenceService = sequenceService;
        }

        public async Task<ServiceResult<string>> AddShipper(ShipperCreateModel model)
        {
            var serviceResult = new ServiceResult<string>
            {
                IsSuccess = false,
                Data = string.Empty,
                Message = Messenger.CreateDataError
            };

            var id = await _sequenceService.GetNextShipperIdAsync();

            Shipper shipper = new Shipper
            {
                PublicId = id,
                Name = model.Name,
                Description = model.Description,
                SupportPhone = model.SupportPhone,
                Website = model.Website,
                LogoUrl = model.LogoUrl,
                IsActive = model.IsActive,

                EntityStatus = EEntityStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            await _uow.Shippers.AddAsync(shipper);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = id;
            serviceResult.Message = Messenger.SuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> DeleteShipper(string id)
        {
            ServiceResult<bool> serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NoExitData
            };

            var shipper = await _uow.Shippers.GetByIdAsync(id);

            if (shipper == null)
            {
                return serviceResult;
            }

            shipper.EntityStatus = EEntityStatus.Deleted;

            _uow.Shippers.Update(shipper);
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

        public async Task<ServiceResult<List<ShipperResponseModel>>> GetAllShippers()
        {
            var serviceResult = new ServiceResult<List<ShipperResponseModel>>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.NoExitData
            };

            var shippers = await _uow.Shippers.GetAllAsync();

            if (shippers == null)
            {
                return serviceResult;
            }

            serviceResult.Data = shippers.ToList().ToListResponseModels();
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;

            return serviceResult;
        }

        public async Task<ServiceResult<ShipperResponseModel>> GetShipperById(string id)
        {
            var serviceResult = new ServiceResult<ShipperResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.NoExitData
            };

            var shipper = await _uow.Shippers.GetByIdAsync(id);

            if (shipper == null)
            {
                return serviceResult;
            }

            serviceResult.Data = shipper.ToResponseModel();
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;

            return serviceResult;
        }

        public async Task<ServiceResult<List<ShipperResponseModel>>> GetShippersByStatus(bool status)
        {
            var serviceResult = new ServiceResult<List<ShipperResponseModel>>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.NoExitData
            };

            var shippers = await _uow.Shippers.FindManyAsync(x => x.IsActive == status);

            if (shippers == null)
            {
                return serviceResult;
            }

            serviceResult.Data = shippers.ToList().ToListResponseModels();
            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateShipper(string id, ShipperUpdateModel model)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NoExitData,
            };

            var shipper = await _uow.Shippers.GetByIdAsync(id);

            if (shipper == null)
            {
                return serviceResult;
            }

            shipper.Name = string.IsNullOrEmpty(model.Name) ? shipper.Name : model.Name;
            shipper.Description = string.IsNullOrEmpty(model.Description) ? shipper.Description : model.Description;
            shipper.Website = string.IsNullOrEmpty(model.Website) ? shipper.Website : model.Website;
            shipper.SupportPhone = string.IsNullOrEmpty(model.SupportPhone) ? shipper.SupportPhone : model.SupportPhone;
            shipper.LogoUrl = string.IsNullOrEmpty(model.LogoUrl) ? shipper.LogoUrl : model.LogoUrl;
            shipper.IsActive = model.IsActive;
            shipper.UpdatedAt = DateTime.UtcNow;

            _uow.Shippers.Update(shipper);
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
    }
}
