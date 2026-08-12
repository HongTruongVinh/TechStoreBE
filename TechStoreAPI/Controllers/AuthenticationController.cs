using Microsoft.AspNetCore.Mvc;
using TechStore.Model.DTOs.Authentication;
using TechStore.Common.Models;
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
        public async Task<ActionResult<ApiResponse<LoginResponseModel>>> Login([FromBody] LoginRequestModel loginModel)
        {
            var serviceResult = await _authenticationService.LoginCustomer(loginModel);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (serviceResult.IsSuccess)
            {
                _logger.LogInformation(
                    AuthLogEvents.LoginSuccess,
                    "Customer login successful. UserId: {UserId}, IP: {IpAddress}",
                    serviceResult.Data?.User.Id,
                    ipAddress);
            }
            else
            {
                _logger.LogWarning(
                    AuthLogEvents.LoginFailed,
                    "Customer login failed. Username: {Username}, IP: {IpAddress}",
                    loginModel.LoginIdentifier,
                    ipAddress);
            }

            return serviceResult.ToActionResult(this);
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
            var userId = User.GetRequiredUserId();

            var serviceResult = await _authenticationService.LogoutAsync(Request.Headers["Authorization"].FirstOrDefault()!);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("change-password")]
        public async Task<ActionResult<ApiResponse<bool>>> ChangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _authenticationService.ChangePasswordAsync(userId, changePasswordModel);

            return serviceResult.ToActionResult(this);
        }
    }
}
