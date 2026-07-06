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
using TechStore.Data.Repositories.Implementations;
using TechStore.Data.Repositories.Interfaces;
using TechStore.Data.UnitOfWork;
using TechStore.Model.DTOs.Authentication;
using TechStore.Model.DTOs.User;
using TechStore.Service.Interfaces;
using TechStore.Service.Mappers;

namespace TechStore.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly SequenceGeneratorService _sequenceService;

        public UserService(IUnitOfWork uow, SequenceGeneratorService sequenceService)
        {
            _uow = uow;
            _sequenceService = sequenceService;
        }

        public async Task<ServiceResult<List<UserListItemResponseModel>>> GetAllUser()
        {
            var serviceResult = new ServiceResult<List<UserListItemResponseModel>>
            {
                IsSuccess = true,
                Data = null,
                Message = Messenger.NoExitData,
            };

            var users = await _uow.Users.GetAllAsync();

            if (users == null)
            {
                return serviceResult;
            }

            serviceResult.Data = users.ToList().ToListUserListItemModels();

            return serviceResult;
        }

        public async Task<ServiceResult<UserResponseModel>> GetByEmail(string email)
        {
            var serviceResult = new ServiceResult<UserResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.NotFoundUser
            };

            var user = await _uow.Users.FindOneAsync(x => x.Email == email);

            if (user == null)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;
            serviceResult.Data = user.ToUserResponseModel(ConvertProjectEnum.ConvertRoleIdToName(user.RoleId));

            return serviceResult;
        }

        public async Task<ServiceResult<UserResponseModel>> GetById(string id)
        {
            var serviceResult = new ServiceResult<UserResponseModel>
            {
                IsSuccess = false,
                Data = null,
                Message = Messenger.NotFoundUser
            };

            var user = await _uow.Users.GetByIdAsync(id);

            if (user == null)
            {
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Message = Messenger.GetDataSuccessful;
            serviceResult.Data = user.ToUserResponseModel(ConvertProjectEnum.ConvertRoleIdToName(user.RoleId));

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> UpdateUserInformation(string id, UserUpdateModel model)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NotFoundUser
            };

            var user = await _uow.Users.GetByIdAsync(id);

            if (user == null)
            {
                return serviceResult;
            }

            user.LastName = model.LastName;
            user.FirstName = model.FirstName;
            user.Email = model.Email;
            user.Gender = model.Gender;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            user.Birthday = model.Birthday;

            _uow.Users.Update(user);
            var result = await _uow.CommitAsync();

            if(result < 1)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> DeleteUser(string id)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NotFoundUser
            };

            var user = await _uow.Users.GetByIdAsync(id);

            if (user == null)
            { 
                return serviceResult;
            }

            user.Status = EUserStatus.Deleted;

            _uow.Users.Update(user);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<bool>> DeleteEntity(string id)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NotFoundUser
            };

            var user = await _uow.Users.GetByIdAsync(id);

            if (user == null)
            {
                return serviceResult;
            }

            user.Status = EUserStatus.Deleted;

            _uow.Users.Remove(user);
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

        public async Task<ServiceResult<bool>> ChangePasswordAsync(string id, ChangePasswordModel changePasswordRequestModel)
        {
            var serviceResult = new ServiceResult<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = Messenger.NotFoundUser
            };

            var user = await _uow.Users.GetByIdAsync(id);

            if (user == null)
            {
                return serviceResult;
            }

            user.PasswordHash = changePasswordRequestModel.NewPassword;

            _uow.Users.Update(user);
            var result = await _uow.CommitAsync();

            if (result < 1)
            {
                serviceResult.Message = Messenger.SystemError;
                return serviceResult;
            }

            serviceResult.IsSuccess = true;
            serviceResult.Data = true;
            serviceResult.Message = Messenger.UpdateSuccessFull;

            return serviceResult;
        }

        public async Task<ServiceResult<List<CustomerListItemModel>>> GetCustomers()
        {
            var serviceResult = new ServiceResult<List<CustomerListItemModel>>
            {
                IsSuccess = true,
                Data = new List<CustomerListItemModel>(),
                Message = Messenger.GetDataSuccessful
            };

            var users = await _uow.Users.FindManyAsync(x => x.RoleId == ERole.Customer);

            if (users == null)
            {
                return serviceResult;
            }

            foreach (var user in users)
            {
                var orders = await _uow.Orders.FindManyAsync(o => o.CustomerId == user.Id);

                if (orders != null)
                {
                    serviceResult.Data.Add(user.ToCustomersLisItemModel(orders.ToList().ToListOrdersHistoryModel()));
                }
            }

            return serviceResult;
        }

        public async Task<ServiceResult<List<StaffListItemModel>>> GetStaffs()
        {
            var serviceResult = new ServiceResult<List<StaffListItemModel>>
            {
                IsSuccess = true,
                Data = new List<StaffListItemModel>(),
                Message = Messenger.GetDataSuccessful
            };

            var users = await _uow.Users.FindManyAsync(x => x.RoleId == ERole.Staff);

            if (users == null)
            {
                return serviceResult;
            }

            foreach (var user in users)
            {
                serviceResult.Data.Add(user.ToStaffLisItemModel());
            }

            return serviceResult;
        }


    }
}
