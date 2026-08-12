using System;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Authentication;
using TechStore.Model.DTOs.User;

namespace TechStore.Service.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<List<UserListItemResponseModel>>> GetAllUser();
        Task<ServiceResult<List<CustomerListItemModel>>> GetCustomers();
        Task<ServiceResult<List<StaffListItemModel>>> GetStaffs();
        Task<ServiceResult<UserResponseModel>> GetByEmail(string email);
        Task<ServiceResult<UserResponseModel>> GetById(string userId);
        Task<ServiceResult<bool>> UpdateUserInformation(string userId, UserUpdateModel model);
        Task<ServiceResult<bool>> DeleteUser(string userId);
        public Task<ServiceResult<bool>> ChangePasswordAsync(string userId, ChangePasswordModel changePasswordRequestModel);
    }
}
