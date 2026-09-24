using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Common.Models;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Authentication;
using TechStore.Model.DTOs.Payment;
using TechStore.Model.DTOs.User;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(
            ILogger<AuthenticationController> logger, 
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserResponseModel>>> Login([FromBody] LoginRequestModel loginModel)
        {
            var serviceResult = await _authenticationService.LoginCustomer(loginModel);

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

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<bool>>> Register([FromBody] CustomerRegisterModel registerModel)
        {
            var serviceResult = await _authenticationService.RegisterCustomer(registerModel);

            return serviceResult.ToActionResult(this);
        }

        [HttpPost("IsEmailExist")]
        public async Task<ActionResult<ApiResponse<bool>>> IsEmailExist([FromBody] AccountExistModel accountExistModel)
        {
            var serviceResult = await _authenticationService.IsUserExist(accountExistModel.Email);

            return serviceResult.ToActionResult(this);
        }

        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponse<bool>>> Logout()
        {
            //var userId = User.GetRequiredUserId();

            var accessToken = Request.Cookies[AuthConstants.AccessTokenCookie];
            var refreshToken = Request.Cookies[AuthConstants.RefreshTokenCookie];

            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                return Unauthorized(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy access token hoặc refresh token.",
                    Data = false
                });
            }

            var serviceResult = await _authenticationService.LogoutAsync(accessToken, refreshToken);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("change-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _authenticationService.ChangePasswordAsync(userId, changePasswordModel);

            return serviceResult.ToActionResult(this);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<ApiResponse<bool>>> RefreshToken(CancellationToken cancellationToken)
        {
            var refreshToken = Request.Cookies[AuthConstants.RefreshTokenCookie];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return Unauthorized(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy refresh token.",
                    Data = false
                });
            }

            var idempotencyKey = HttpContext.Request.Headers["Idempotency-Key"].FirstOrDefault();

            if (idempotencyKey == null)
            {
                return BadRequest(new ApiResponse<bool> { Success = false, Message = "Idempotency-Key header is required" });
            }

            var result = await _authenticationService
                .GenerateNewAccessTokenAsync( new RefreshTokenRequest
                { 
                    IdempotencyKey = idempotencyKey,
                    RefreshToken = refreshToken
                }, cancellationToken);

            if (!result.IsSuccess || result.Data is null)
            {
                return Unauthorized(new ApiResponse<bool>
                {
                    Success = false,
                    Message = result.Message,
                    Data = false
                });
            }

            /*
             * Set Refresh Token mới vào HttpOnly Cookie
             */
            Response.Cookies.Append(
                AuthConstants.AccessTokenCookie,
                result.Data.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.Data.AccessTokenExpiresAt,
                    Path = "/"
                });

            Response.Cookies.Append(
                AuthConstants.RefreshTokenCookie,
                result.Data.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.Data.RefreshTokenExpiresAt,
                    Path = AuthConstants.AuthenticationPath
                });

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Refresh token thành công.",
                Data = true
            });
        }
    }
}
