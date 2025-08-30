using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Authentication;
using TechStore.Model.DTOs.User;

namespace TechStore.Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ServiceResult<string>> Register(RegisterModel registerModel);
        Task<ServiceResult<LoginResponseModel>> CustomerLogin(LoginRequestModel loginModel);
        Task<ServiceResult<LoginResponseModel>> AdminLogin(LoginRequestModel loginModel);


        Task<ServiceResult<string>> AdminRegisterWithEmail(RegisterModel registerModel);
        Task<ServiceResult<bool>> IsUserExist(string identifier);
        Task<ServiceResult<bool>> UpdateUserRole(UserRoleUpdateModel model);

        Task<ServiceResult<string>> UserRegisterWithEmail(RegisterModel registerModel);

        Task<ServiceResult<bool>> LogoutAsync(string token);
    }
}
