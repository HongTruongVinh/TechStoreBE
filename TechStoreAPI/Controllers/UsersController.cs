using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Authentication;
using TechStore.Model.DTOs.User;
using TechStore.Service.Interfaces;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<ApiResponse<UserResponseModel>> GetUser(string userId)
        {
            //var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _userService.GetById(userId);

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
                        PartnerCode = Messenger.NoExitData,
                        RetCode = ERetCode.NoExitData,
                        Data = serviceResult.Data,
                        SystemMessage = serviceResult.Message,
                        StatusCode = (int)HttpStatusCode.OK
                    };
                }
            }
            else
            {
                return new()
                {
                    PartnerCode = Messenger.LoginError,
                    RetCode = ERetCode.LoginError,
                    Data = null,
                    SystemMessage = Messenger.LoginError,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }

        [HttpPut("update-information/{id}")]
        public async Task<ApiResponse<bool>> UpdateUser(string id, UserUpdateModel model)
        {
            var serviceResult = await _userService.UpdateUserInformation(id, model);

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
                    PartnerCode = Messenger.NoExitData,
                    RetCode = ERetCode.NoExitData,
                    Data = serviceResult.Data,
                    SystemMessage = serviceResult.Message,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }

        [HttpPut("change-password/{userId}")]
        public async Task<ApiResponse<bool>> ChangePassword(string userId, ChangePasswordRequestModel model)
        {
            //var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _userService.ChangePasswordAsync(userId, model);

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
                        PartnerCode = Messenger.NoExitData,
                        RetCode = ERetCode.NoExitData,
                        Data = serviceResult.Data,
                        SystemMessage = serviceResult.Message,
                        StatusCode = (int)HttpStatusCode.OK
                    };
                }
            }
            else
            {
                ApiResponse<bool> result = new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = false,
                    SystemMessage = "Fail to change password",
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };

                return result;
            }
        }
    }
}
