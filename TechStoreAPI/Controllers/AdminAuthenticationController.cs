using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Authentication;
using TechStore.Model.DTOs.User;
using TechStore.Service.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechStoreAPI.Controllers
{
    [Route(RouterControllerName.AdminAuthentication)]
    [ApiController]
    public class AdminAuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly ILogger<AdminAuthenticationController> _logger;

        public AdminAuthenticationController(
            IAuthenticationService authenticationService
            , ILogger<AdminAuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ApiResponse<LoginResponseModel>> Login(LoginRequestModel loginModel)
        {
            ServiceResult<LoginResponseModel> serviceResult = await _authenticationService.AdminLogin(loginModel);

            if (serviceResult.IsSuccess)
            {
                return new ApiResponse<LoginResponseModel>
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.LoginSuccess,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else if (serviceResult.ErrorCode == "NoPermission")
            {
                return new ApiResponse<LoginResponseModel>
                {
                    PartnerCode = Messenger.NoPermission,
                    RetCode = ERetCode.NoPermission,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new()
                {
                    PartnerCode = Messenger.BadRequest,
                    RetCode = ERetCode.LoginError,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [HttpPost("register")]
        public async Task<ApiResponse<string>> Register([FromBody] RegisterModel registerModel)
        {
            ServiceResult<string> serviceResult = await _authenticationService.Register(registerModel);

            if (serviceResult.IsSuccess)
            {
                return new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new()
                {
                    PartnerCode = Messenger.BadRequest,
                    RetCode = ERetCode.BadRequest,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }


        //[Authorize]
        [HttpPost("update-role")]
        public async Task<ApiResponse<bool>> UpdateUserRole([FromBody] UserRoleUpdateModel model)
        {
            ServiceResult<bool> serviceResult = await _authenticationService.UpdateUserRole(model);

            if (serviceResult.IsSuccess)
            {
                return new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new()
                {
                    PartnerCode = Messenger.BadRequest,
                    RetCode = ERetCode.BadRequest,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }

        [HttpPost("IsEmailExist")]
        public async Task<ApiResponse<bool>> IsEmailExist([FromBody] AccountExistModel accountExistModel)
        {
            ServiceResult<bool> serviceResult = await _authenticationService.IsUserExist(accountExistModel.Email);

            if (serviceResult.IsSuccess)
            {
                return new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                return new()
                {
                    PartnerCode = Messenger.BadRequest,
                    RetCode = ERetCode.BadRequest,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
    }
}
