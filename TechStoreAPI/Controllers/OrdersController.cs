using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Payment;
using TechStore.Model.DTOs.Snapshot;
using TechStore.Service.Implementations;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;

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

        [Authorize(Roles = AppRoles.Customer)]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ListItemOrderModel>>>> GetUserOrders(int page = 1, int pageSize = 1000)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.GetCustomerOrdersAsync(userId, page, pageSize);

            return serviceResult.ToActionResult(this);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = AppRoles.Customer)]
        public async Task<ActionResult<ApiResponse<OrderDetailResponseModel>>> GetOrderById(string id)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.GetOrderByIdAsync(userId, id);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<ApiResponse<bool>>> CancelOrder(string id, CancelOrderModel model)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.CancelOrderByCustomerAsync(userId, id, model);

            return serviceResult.ToActionResult(this);
        }

        [Authorize]
        [HttpPost("create-snapshot-order")]
        public async Task<ActionResult<ApiResponse<CreatePaymentSnapshotResult>>> CreateSnapshotOrderAsync(OrderCreateModel createOrderRequest)
        {
            var userId = User.GetRequiredUserId();

            var idempotencyKey = HttpContext.Request.Headers["Idempotency-Key"].FirstOrDefault();

            if (idempotencyKey == null)
            {
                return BadRequest(new ApiResponse<PaymentDataForSnapshotModel> { Success = false, Message = "Idempotency-Key header is required" });
            }

            var serviceResult = await _orderService.CreateSnapshotAsync(userId, createOrderRequest, idempotencyKey);

            return serviceResult.ToActionResult(this);
        }

        [HttpPost("cod-order")]
        public async Task<ActionResult<ApiResponse<CreateCODOnlineOrderResult>>> CreateCODOnlineOrderAsync(OrderCreateModel orderCreateModel)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.CreateCODOnlineOrderAsync(userId, orderCreateModel);

            return serviceResult.ToActionResult(this);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrder(string id, UpdateOrderModel model)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _orderService.UpdateOrderByCustomerAsync(userId, id, model);

            return serviceResult.ToActionResult(this);
        }
    }
}
