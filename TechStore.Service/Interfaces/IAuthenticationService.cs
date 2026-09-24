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
        Task<ServiceResult<bool>> RegisterCustomer(CustomerRegisterModel registerModel);
        Task<ServiceResult<LoginResponseModel>> LoginCustomer(LoginRequestModel loginModel);
        Task<ServiceResult<LoginResponseModel>> LoginAdmin(LoginRequestModel loginModel);
        Task<ServiceResult<RefreshTokenRotationResult>> GenerateNewAccessTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);


        Task<ServiceResult<string>> RegisterAdminByEmail(RegisterModel registerModel);
        Task<ServiceResult<bool>> IsUserExist(string identifier);
        Task<ServiceResult<bool>> UpdateUserRole(UserRoleUpdateModel model);

        Task<ServiceResult<bool>> LogoutAsync(string accesstoken, string refreshToken);
        Task<ServiceResult<bool>> ChangePasswordAsync(string userId, ChangePasswordModel model);
        Task<bool> IsInvalidAsync(string jti); 
    }
}
