using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Common.Enums;
using TechStore.Model.DTOs.Order;
using TechStore.Common.Models;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

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
        public async Task<ActionResult<ApiResponse<PagedResult<ListItemOrderModel>>>> GetOrders([FromQuery] OrderSearchQuery query)
        {
            var serviceResult = await _orderService.GetOrdersAsync(query);

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("status")]
        public async Task<ActionResult<ApiResponse<PagedResult<OrderDetailResponseModel>>>> GetListOrderByStatusId(
            [FromQuery] EOrderStatus status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var serviceResult = await _orderService.GetListOrdersByStatusIdAsync(status, page, pageSize);

            return serviceResult.ToActionResult(this);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderDetailResponseModel>>> GetOrderById(string id)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.AdminGetOrderByIdAsync(userId, id);

            return serviceResult.ToActionResult(this);
        }

        [Authorize]
        [HttpPut("cancel/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderStatusToCanceled(string id, CancelOrderModel model)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.CancelOrderByAdminAsync(userId, id, model);

            return serviceResult.ToActionResult(this);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("processing/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderStatusToProcessing(string id)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.UpdateOrderStatusToProcessingAsync(userId, id);

            return serviceResult.ToActionResult(this);
        }

        [Authorize]
        [HttpPut("delivering/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderStatusToDelivering(string id, UpdateOrderToDeliveringModel model)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.UpdateOrderStatusToDeliveringAsync(userId, id, model);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("completed/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderStatusToCompleted(string id)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.UpdateOrderStatusToCompletedAsync(userId, id);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("failed/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderStatusToFailed(string id, OrderUpdateStatusModel model)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.UpdateOrderStatusToFailedAsync(userId, id, model);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("refunded/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderStatusToRefunded(string id, OrderUpdateStatusModel model)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.UpdateOrderStatusToRefundedAsync(userId, id, model);

            return serviceResult.ToActionResult(this);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteOrder(string id)
        {
            var serviceResult = await _orderService.DeleteOrderAsync(id);

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("instore-orders")]
        public async Task<ActionResult<ApiResponse<List<ListItemOrderModel>>>> GetStoreOrder()
        {
            var serviceResult = await _orderService.GetInStoreOrdersAsync();

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("instore-orders/{id}")]
        public async Task<ActionResult<ApiResponse<InStoreOrderResponseModel>>> GetStoreOrder(string id)
        {
            var serviceResult = await _orderService.GetInStoreOrderAsync(id);

            return serviceResult.ToActionResult(this);
        }

        [HttpPost("instore-orders")]
        public async Task<ActionResult<ApiResponse<string>>> AddInStoreOrder(InStoreOrderCreateModel model)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.CreateInStoreOrderAsync(userId, "", model);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("instore-orders/confirm-instore-order/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> ConfirmInStoreOrder(string id)
        {
            var serviceResult = await _orderService.ConfirmInStoreOrder(id);

            return serviceResult.ToActionResult(this);
        }
    }
}
