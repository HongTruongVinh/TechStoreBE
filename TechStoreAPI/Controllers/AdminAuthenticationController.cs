using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Authentication;
using TechStore.Model.DTOs.User;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route(RouterControllerName.AdminAuthentication)]
    [ApiController]
    public class AdminAuthenticationController : ControllerBase
    {
        private readonly ILogger<AdminAuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AdminAuthenticationController(
            ILogger<AdminAuthenticationController> logger,
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserResponseModel>>> Login([FromBody] LoginRequestModel loginModel)
        {
            var serviceResult = await _authenticationService.LoginAdmin(loginModel);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (!serviceResult.IsSuccess || serviceResult.Data is null)
            {
                _logger.LogWarning(
                    AuthLogEvents.LoginFailed,
                    "Customer login failed. Username: {Username}, IP: {IpAddress}",
                    loginModel.LoginIdentifier,
                    ipAddress);

                return NotFound(new ApiResponse<UserResponseModel>
                {
                    Success = false,
                    Message = serviceResult.Message,
                    Data = null
                });
            }
            else
            {
                _logger.LogInformation(
                    AuthLogEvents.LoginSuccess,
                    "Customer login successful. UserId: {UserId}, IP: {IpAddress}",
                    serviceResult.Data?.User.Id,
                    ipAddress);
            }

            Response.Cookies.Append(
                AuthConstants.AccessTokenCookie,
                serviceResult.Data!.RefreshTokenRotationResult.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = serviceResult.Data.RefreshTokenRotationResult.AccessTokenExpiresAt,
                    Path = "/"
                });

            Response.Cookies.Append(
                AuthConstants.RefreshTokenCookie,
                serviceResult.Data.RefreshTokenRotationResult.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = serviceResult.Data.RefreshTokenRotationResult.RefreshTokenExpiresAt,
                    Path = AuthConstants.AuthenticationPath
                });

            return new ApiResponse<UserResponseModel>
            {
                Success = true,
                Message = serviceResult.Message,
                Data = serviceResult.Data.User
            };
        }
    }
}
