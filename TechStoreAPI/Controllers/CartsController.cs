//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using System.Net;
//using System.Security.Claims;
//using TechStoreData.Entities;
//using TechStoreModel.Base;
//using TechStoreModel.Constants;
//using TechStoreModel.Enums;
//using TechStoreModel.Model.Cart;
//using TechStoreServices.IServices;
//using TechStoreServices.Services;

//namespace TechStoreAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CartsController : ControllerBase
//    {
//        private readonly ICartService _cartService;

//        public CartsController(ICartService cartService)
//        {
//            _cartService = cartService;
//        }

//        //[Authorize]
//        [HttpGet("{userId}")]
//        public async Task<ApiResponse<IEnumerable<CartItemResponseModel>>> GetProducts(string userId)
//        {
//            //var userId = User.FindFirstValue(AppClaims.UserId);

//            if (userId != null)
//            {
//                var serviceResult = await _cartService.GetCartItems(userId);

//                if (serviceResult.IsSuccess)
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.SuccessFull,
//                        RetCode = ERetCode.Successfull,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//                else
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.NoExitData,
//                        RetCode = ERetCode.NoExitData,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//            }
//            else
//            {
//                ApiResponse<IEnumerable<CartItemResponseModel>> result = new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = null,
//                    SystemMessage = Messenger.LoginError,
//                    StatusCode = (int)HttpStatusCode.ExpectationFailed
//                };

//                return result;
//            }
//        }

//        [HttpPost("{userId}")]
//        public async Task<ApiResponse<bool>> AddProductToCart(string userId, CartItemUpdateModel model)
//        {
//            //var userId = User.FindFirstValue(AppClaims.UserId);

//            if (userId != null)
//            {
//                var serviceResult = await _cartService.AddToCart(userId, model);

//                if (serviceResult.IsSuccess)
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.SuccessFull,
//                        RetCode = ERetCode.Successfull,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//                else
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.NoExitData,
//                        RetCode = ERetCode.NoExitData,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//            }
//            else
//            {
//                ApiResponse<bool> result = new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = false,
//                    SystemMessage = Messenger.LoginError,
//                    StatusCode = (int)HttpStatusCode.FailedDependency
//                };

//                return result;
//            }
//        }

//        [HttpPut("clear/{userId}")]
//        public async Task<ApiResponse<bool>> ClearCart(string userId)
//        {
//            //var userId = User.FindFirstValue(AppClaims.UserId);

//            if (userId != null)
//            {
//                var serviceResult = await _cartService.ClearCart(userId);

//                if (serviceResult.IsSuccess)
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.SuccessFull,
//                        RetCode = ERetCode.Successfull,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//                else
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.NoExitData,
//                        RetCode = ERetCode.NoExitData,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//            }
//            else 
//            {
//                ApiResponse<bool> result = new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = false,
//                    SystemMessage = Messenger.LoginError,
//                    StatusCode = (int)HttpStatusCode.ExpectationFailed
//                };

//                return result;
//            }
//        }

//        [HttpPut("remove/{userId}")]
//        public async Task<ApiResponse<bool>> RemoveProduct(string userId, CartItemUpdateModel model)
//        {
//            //var userId = User.FindFirstValue(AppClaims.UserId);

//            if (userId != null)
//            {
//                var serviceResult = await _cartService.RemoveFromCart(userId, model);

//                if (serviceResult.IsSuccess)
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.SuccessFull,
//                        RetCode = ERetCode.Successfull,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//                else
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.NoExitData,
//                        RetCode = ERetCode.NoExitData,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//            }
//            else
//            {
//                ApiResponse<bool> result = new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = false,
//                    SystemMessage = string.Empty,
//                    StatusCode = (int)HttpStatusCode.ExpectationFailed
//                };

//                return result;
//            }

//        }

//        [HttpPut("{userId}")]
//        public async Task<ApiResponse<bool>> UpdateCart(string userId, CartItemUpdateModel model)
//        {
//            //var userId = User.FindFirstValue(AppClaims.UserId);

//            if (userId != null)
//            {
//                var serviceResult = await _cartService.UpdateCart(userId, model);

//                if (serviceResult.IsSuccess)
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.SuccessFull,
//                        RetCode = ERetCode.Successfull,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//                else
//                {
//                    return new()
//                    {
//                        PartnerCode = Messenger.NoExitData,
//                        RetCode = ERetCode.NoExitData,
//                        Data = serviceResult.Data,
//                        SystemMessage = serviceResult.Message,
//                        StatusCode = (int)HttpStatusCode.OK
//                    };
//                }
//            }
//            else
//            {
//                ApiResponse<bool> result = new()
//                {
//                    PartnerCode = Messenger.SuccessFull,
//                    RetCode = ERetCode.Successfull,
//                    Data = false,
//                    SystemMessage = Messenger.LoginError,
//                    StatusCode = (int)HttpStatusCode.ExpectationFailed
//                };

//                return result;
//            }
//        }
//    }
//}
