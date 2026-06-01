using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Payment;
using TechStore.Service.Interfaces;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ApiResponse<List<OrderListItemModel>>> GetListOrder(string userId)
        {
            //var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.GetCustomerOrdersAsync(userId);

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
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = null,
                    SystemMessage = Messenger.LoginError,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };
            }
        }

        [HttpGet]
        public async Task<ApiResponse<List<OrderListItemModel>>> GetUserOrders()
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.GetCustomerOrdersAsync(userId);

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
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = null,
                    SystemMessage = Messenger.LoginError,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };
            }
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<OrderDetailResponseModel>> GetOrderById(string id)
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.GetOrderByIdAsync(userId, id);

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
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = null,
                    SystemMessage = Messenger.LoginError,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };
            }
        }

        [HttpPost("{id}/cancel/{userId}")]
        public async Task<ApiResponse<bool>> CancelOrder(string userId, string id, CancelOrderModel model)
        {
            //var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.UpdateOrderStatusToCanceledAsync(userId, id, model);

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
                    SystemMessage = Messenger.LoginError,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };

                return result;
            }
        }

        [HttpPost("cod-order")]
        public async Task<ApiResponse<string>> CreateCODOnlineOrderAsync(OrderCreateModel orderCreateModel)
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.CreateCODOnlineOrderAsync(userId, orderCreateModel);

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
                ApiResponse<string> result = new()
                {
                    PartnerCode = Messenger.SuccessFull,
                    RetCode = ERetCode.Successfull,
                    Data = null,
                    SystemMessage = Messenger.LoginError,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };

                return result;
            }
        }

        //[HttpPost("prepay-order")]
        //public async Task<ApiResponse<PaymentDataModel>> CreatePrePayOnlineOrderAsync(OrderCreateModel orderCreateModel)
        //{
        //    var userId = User.FindFirstValue(AppClaims.UserId);

        //    if (userId != null)
        //    {
        //        var serviceResult = await _orderService.CreatePrePayOnlineOrderAsync(userId, orderCreateModel);

        //        if (serviceResult.IsSuccess)
        //        {
        //            return new()
        //            {
        //                PartnerCode = Messenger.SuccessFull,
        //                RetCode = ERetCode.Successfull,
        //                Data = serviceResult.Data,
        //                SystemMessage = serviceResult.Message,
        //                StatusCode = (int)HttpStatusCode.OK
        //            };
        //        }
        //        else
        //        {
        //            return new()
        //            {
        //                PartnerCode = Messenger.NoExitData,
        //                RetCode = ERetCode.NoExitData,
        //                Data = serviceResult.Data,
        //                SystemMessage = serviceResult.Message,
        //                StatusCode = (int)HttpStatusCode.OK
        //            };
        //        }
        //    }
        //    else
        //    {
        //        ApiResponse<PaymentDataModel> result = new()
        //        {
        //            PartnerCode = Messenger.SuccessFull,
        //            RetCode = ERetCode.Successfull,
        //            Data = null,
        //            SystemMessage = Messenger.LoginError,
        //            StatusCode = (int)HttpStatusCode.ExpectationFailed
        //        };

        //        return result;
        //    }
        //}
    }
}
