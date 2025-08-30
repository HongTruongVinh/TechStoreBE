//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using System.Security.Claims;
//using TechStoreModel.Base;
//using TechStoreModel.Constants;
//using TechStoreModel.Enums;
//using TechStoreModel.Model;
//using TechStoreModel.Model.Authentication;
//using TechStoreServices.IServices;

//namespace TechStoreAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthenticationController : ControllerBase
//    {
//        private readonly IAuthenticationService _authenticationService;

//        public AuthenticationController(IAuthenticationService authenticationService)
//        {
//            _authenticationService = authenticationService;
//        }

//        [HttpPost("login")]
//        public async Task<ApiResponse<LoginResponseModel>> Login([FromBody] LoginRequestModel loginModel)
//        {
//            ServiceResult<LoginResponseModel> serviceResult = await _authenticationService.CustomerLogin(loginModel);

//            if (serviceResult.IsSuccess)
//            {
//                return new ApiResponse<LoginResponseModel>
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.LoginSuccess,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//            else
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.BadRequest,
//                    RetCode = ERetCode.LoginError,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.BadRequest
//                };
//            }
//        }

//        [HttpPost("register")]
//        public async Task<ApiResponse<RegisterResponseModel>> Register([FromBody] RegisterModel registerModel)
//        {
//            ServiceResult<RegisterResponseModel> serviceResult = await _authenticationService.Register(registerModel);

//            if (serviceResult.IsSuccess)
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//            else
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.BadRequest,
//                    RetCode = ERetCode.BadRequest,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.BadRequest
//                };
//            }
//        }

//        [HttpPost("IsEmailExist")]
//        public async Task<ApiResponse<bool>> IsEmailExist([FromBody] AccountExistModel accountExistModel)
//        {
//            ServiceResult<bool> serviceResult = await _authenticationService.IsUserExist(accountExistModel.Email);

//            if (serviceResult.IsSuccess)
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.OK
//                };
//            }
//            else
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.BadRequest,
//                    RetCode = ERetCode.BadRequest,
//                    Data = serviceResult.Data,
//                    SystemMessage = serviceResult.Message,
//                    StatusCode = (int)HttpStatusCode.BadRequest
//                };
//            }
//        }

//        [HttpPost("logout")]
//        public async Task<ApiResponse<bool>> Logout()
//        {
//            var userId = User.FindFirstValue(AppClaims.UserId);

//            string? token = Request.Headers["Authorization"].FirstOrDefault();

//            if (userId != null && !string.IsNullOrEmpty(token))
//            {
//                var serviceResult = await _authenticationService.LogoutAsync(token);

//                if (serviceResult.IsSuccess)
//                {
//                    return new ApiResponse<bool>
//                    {
//                        PartnerCode = Messenger.SuccessFull,
//                        RetCode = ERetCode.LoginSuccess,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//                else
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.BadRequest,
//                        RetCode = ERetCode.LoginError,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.BadRequest
//                    };
//                }
//            }
//            else
//            {
//                return new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = false,
//                    SystemMessage = Messenger.LoginError,
//                    StatusCode = (int)HttpStatusCode.ExpectationFailed
//                };
//            }
//        }
//    }
//}
