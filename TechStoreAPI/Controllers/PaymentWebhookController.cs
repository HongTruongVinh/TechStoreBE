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
        private readonly IHubContext<PaymentHub> _hubContext;
        private readonly IPaymentService _paymentService;

        public PaymentWebhookController(
            IHubContext<PaymentHub> hubContext,
            IPaymentService paymentService)
        {
            _hubContext = hubContext;
            _paymentService = paymentService;
        }

        [HttpPost("verify-payment-of-snapshot")]
        public async Task<IActionResult> VerifyPaymenForSnapshottWebhook(PaymentForSnapshotWebhookRequest request)
        {
            var result = await _paymentService.VerifyPaymentForSnapshotAsync(request);

            if (result.IsSuccess)
            {
                await _hubContext
                    .Clients
                    .Group(request.SnapshotId)
                    .SendAsync("PaymentSuccess", new
                    {
                        paymentId = request.SnapshotId,
                        amount = request.Amount,
                        message = "Thanh toán thành công"
                    });
            }
            else
            {
                await _hubContext
                    .Clients
                    .Group(request.SnapshotId)
                    .SendAsync("PaymentFailed", new
                    {
                        paymentId = request.SnapshotId,
                        amount = request.Amount,
                        message = result.Message ?? "Thanh toán thất bại"
                    });
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
                        message = result.Message ?? "Thanh toán thất bại"
                    });
            }

            return Ok();
        }
    }
}
