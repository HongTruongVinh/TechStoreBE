using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Order;
using TechStore.Service.Interfaces;

namespace TechStoreAPI.Controllers
{
    [Route(RouterControllerName.AdminOrders)]
    [ApiController]
    public class AdminOrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public AdminOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ApiResponse<List<OrderListItemModel>>> GetListOrder()
        {
            var serviceResult = await _orderService.GetOnlineOrdersAsync();

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

        [HttpGet("status")]
        public async Task<ApiResponse<PagedResult<OrderDetailResponseModel>>> GetListOrderByStatusId(
            [FromQuery] EOrderStatus status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
            )
        {
            var serviceResult = await _orderService.GetListOrdersByStatusIdAsync(status, page, pageSize);

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

        [HttpGet("{id}")]
        public async Task<ApiResponse<OrderDetailResponseModel>> GetOrderById(string id)
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.AdminGetOrderByIdAsync(userId, id);

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
                ApiResponse<OrderDetailResponseModel> result = new()
                {
                    PartnerCode = Messenger.SystemError,
                    RetCode = ERetCode.SystemError,
                    Data = null,
                    SystemMessage = string.Empty,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };

                return result;
            }
            

        }

        [HttpPut("cancel/{id}")]
        public async Task<ApiResponse<bool>> UpdateOrderStatusToCanceled(string id, CancelOrderModel model)
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

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
                    PartnerCode = Messenger.SystemError,
                    RetCode = ERetCode.SystemError,
                    Data = false,
                    SystemMessage = string.Empty,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };

                return result;
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("processing/{id}")]
        public async Task<ApiResponse<bool>> UpdateOrderStatusToProcessing(string id)
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.UpdateOrderStatusToProcessingAsync(userId, id);

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
                    PartnerCode = Messenger.SystemError,
                    RetCode = ERetCode.SystemError,
                    Data = false,
                    SystemMessage = string.Empty,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };

                return result;
            }
        }

        [HttpPut("delivering/{id}")]
        public async Task<ApiResponse<bool>> UpdateOrderStatusToDelivering(string id, UpdateOrderToDeliveringModel model)
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.UpdateOrderStatusToDeliveringAsync(userId, id, model);

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
                    PartnerCode = Messenger.SystemError,
                    RetCode = ERetCode.SystemError,
                    Data = false,
                    SystemMessage = string.Empty,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };

                return result;
            }
        }

        [HttpPut("completed/{id}")]
        public async Task<ApiResponse<bool>> UpdateOrderStatusToCompeted(string id)
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.UpdateOrderStatusToCompletedAsync(userId, id);

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
                    PartnerCode = Messenger.SystemError,
                    RetCode = ERetCode.SystemError,
                    Data = false,
                    SystemMessage = string.Empty,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };

                return result;
            }
        }

        [HttpPut("failed/{id}")]
        public async Task<ApiResponse<bool>> UpdateOrderStatusToFailed(string id, OrderUpdateStatusModel model)
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.UpdateOrderStatusToFailedAsync(userId, id, model);

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
                    SystemMessage = string.Empty,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };

                return result;
            }
        }

        [HttpPut("refunded/{id}")]
        public async Task<ApiResponse<bool>> UpdateOrderStatusToRefunded(string id, OrderUpdateStatusModel model)
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.UpdateOrderStatusToRefundedAsync(userId, id, model);

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
                    SystemMessage = string.Empty,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };

                return result;
            }
        }

        //[Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse<bool>> DeleteOrder(string id)
        {
            var serviceResult = await _orderService.DeleteOrderAsync(id);

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

        [HttpGet("instore-orders")]
        public async Task<ApiResponse<List<OrderListItemModel>>> GetStoreOrder()
        {

            var serviceResult = await _orderService.GetInStoreOrdersAsync();

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

        [HttpGet("instore-orders/{id}")]
        public async Task<ApiResponse<InStoreOrderResponseModel>> GetStoreOrder(string id)
        {

            var serviceResult = await _orderService.GetInStoreOrderAsync(id);

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

        [HttpPost("instore-orders")]
        public async Task<ApiResponse<string>> AddInStoreOrder(InStoreOrderCreateModel model)
        {
            var userId = User.FindFirstValue(AppClaims.UserId);

            if (userId != null)
            {
                var serviceResult = await _orderService.CreateInStoreOrderAsync(userId, "",model);

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
                    Data = string.Empty,
                    SystemMessage = string.Empty,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed
                };

                return result;
            }
        }

        [HttpPut("instore-orders/confirm-instore-order/{id}")]
        public async Task<ApiResponse<bool>> ConfirmInStoreOrder(string id)
        {

            var serviceResult = await _orderService.ConfirmInStoreOrder(id);

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
    }
}
