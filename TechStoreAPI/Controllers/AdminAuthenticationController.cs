using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Authentication;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

namespace TechStoreAPI.Controllers
{
    [Route(RouterControllerName.AdminAuthentication)]
    [ApiController]
    public class AdminAuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AdminAuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseModel>>> Login(LoginRequestModel loginModel)
        {
            var serviceResult = await _authenticationService.LoginAdmin(loginModel);

            return serviceResult.ToActionResult(this);
        }
    }
}
