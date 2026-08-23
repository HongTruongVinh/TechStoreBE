using Microsoft.AspNetCore.Mvc;
using TechStore.Common.Constants;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Payment;
using TechStore.Common.Models;
using TechStore.Service.Interfaces;
using TechStoreAPI.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace TechStoreAPI.Controllers
{
    [Route(RouterControllerName.Payments)]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<PaymentResponseModel>>>> GetPayments()
        {
            var serviceResult = await _paymentService.GetPayments();

            return serviceResult.ToActionResult(this);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PaymentResponseModel>>> GetPayment(string id)
        {
            var serviceResult = await _paymentService.GetPayment(id);

            return serviceResult.ToActionResult(this);
        }

        //[Authorize]
        //[HttpPost("create-payment-pre-order")]
        //public async Task<ActionResult<ApiResponse<PaymentDataForSnapshotModel>>> CreatePaymentForSnapshot(OrderCreateModel createOrderRequest)
        //{
        //    var userId = User.GetRequiredUserId();

        //    var idempotencyKey = HttpContext.Request.Headers["Idempotency-Key"].FirstOrDefault();

        //    if (idempotencyKey == null)
        //    {
        //        return BadRequest(new ApiResponse<PaymentDataForSnapshotModel> { Success = false, Message = "Idempotency-Key header is required" });
        //    }

        //    var serviceResult = await _paymentService.CreateSnapshotAsync(userId, createOrderRequest, idempotencyKey);

        //    return serviceResult.ToActionResult(this);
        //}

        [Authorize]
        [HttpGet("get-payment-qr-for-snapshot/{snapshotId}")]
        public async Task<ActionResult<ApiResponse<PaymentDataForSnapshotModel>>> GetPaymentQrForSnapshot(string snapshotId)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _paymentService.GenerateSnapshotPaymentQrAsync(userId, snapshotId);

            return serviceResult.ToActionResult(this);
        }

        [Authorize]
        [HttpPost("create-payment-for-invoice")]
        public async Task<ActionResult<ApiResponse<PaymentDataModel>>> CreatePaymentForInvoice(PaymentCreateModel model)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _paymentService.CreatePaymentForInvoiceByAdminAsync(userId, model);

            return serviceResult.ToActionResult(this);
        }

        [Authorize]
        [HttpPost("add-cash-payment")]
        public async Task<ActionResult<ApiResponse<string>>> AddCashPayment([FromBody] CashPaymentCreateModel cashPayment)
        {
            var userId = User.GetRequiredUserId();

            var serviceResult = await _paymentService.AddCashPaymentByAdminAsync(userId, cashPayment);

            return serviceResult.ToActionResult(this);
        }
    }
}
