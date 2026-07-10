using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Techstore.API.Hubs;
using TechStore.Model.DTOs.Payment;
using TechStore.Service.Interfaces;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentWebhookController : ControllerBase
    {
        private readonly ILogger<PaymentWebhookController> _logger;
        private readonly IHubContext<PaymentHub> _hubContext;
        private readonly IPaymentService _paymentService;

        public PaymentWebhookController(
            ILogger<PaymentWebhookController> logger,
            IHubContext<PaymentHub> hubContext,
            IPaymentService paymentService)
        {
            _logger = logger;
            _hubContext = hubContext;
            _paymentService = paymentService;
        }

        //[HttpPost("verify-payment-of-snapshot")]
        //public async Task<IActionResult> Verify()
        //{
        //    using var reader = new StreamReader(Request.Body);
        //    var body = await reader.ReadToEndAsync();

        //    _logger.LogInformation("Webhook received: {RequestBody}", body);

        //    return Ok();
        //}

        [HttpPost("verify-payment-of-snapshot")]
        public async Task<IActionResult> VerifyPaymenForSnapshottWebhook(SepayWebhookRequest request)
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            _logger.LogInformation("SePay Webhook received: {RequestBody}", body);

            var result = await _paymentService.VerifyPaymentForSnapshotAsync(request);

            if(result.Data != null)
            {
                if (result.IsSuccess)
                {
                    await _hubContext
                        .Clients
                        .Group(result.Data.SnapshotId)
                        .SendAsync("PaymentSuccess", new
                        {
                            paymentId = result.Data.SnapshotId,
                            amount = result.Data.Amount,
                            message = result.Data.Message
                        });
                }
                else
                {
                    await _hubContext
                        .Clients
                        .Group(result.Data.SnapshotId)
                        .SendAsync("PaymentFailed", new
                        {
                            paymentId = result.Data.SnapshotId,
                            amount = result.Data.Amount,
                            message = result.Data.Message 
                        });
                }
            }
            else
            {
                _logger.LogWarning("No snapshot data found for the payment verification.");
            }

            return Ok();
        }

        [HttpPost("verify-payment")]
        public async Task<IActionResult> VerifyPaymenForInvoicetWebhook(PaymentForInvocieWebhookRequest request)
        {
            var result = await _paymentService.VerifyPaymentForInvoiceAsync(request);

            if (result.IsSuccess)
            {
                await _hubContext
                    .Clients
                    .Group(request.PaymentId)
                    .SendAsync("PaymentSuccess", new
                    {
                        paymentId = request.PaymentId,
                        amount = request.Amount,
                        message = "Thanh toán thành công"
                    });
            }
            else
            {
                await _hubContext
                    .Clients
                    .Group(request.PaymentId)
                    .SendAsync("PaymentFailed", new
                    {
                        paymentId = request.PaymentId,
                        amount = request.Amount,
                        message = result.Data?.Message ?? "Thanh toán thất bại"
                    });
            }

            return Ok();
        }
    }
}
