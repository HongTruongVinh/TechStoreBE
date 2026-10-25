using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Techstore.API.Hubs;
using TechStore.Common.Constants;
using TechStore.Model.DTOs.Payment;
using TechStore.Service.Interfaces;
using TechStoreAPI.Hubs;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentWebhookController : ControllerBase
    {
        private readonly ILogger<PaymentWebhookController> _logger;
        private readonly IHubContext<PaymentHub> _hubContext;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentNotificationService _paymentNotificationService;
        private readonly IOrderService _orderService;

        public PaymentWebhookController(
            ILogger<PaymentWebhookController> logger,
            IHubContext<PaymentHub> hubContext,
            IPaymentService paymentService,
            IOrderService orderService,
            IPaymentNotificationService paymentNotificationService)
        {
            _logger = logger;
            _hubContext = hubContext;
            _paymentService = paymentService;
            _orderService = orderService;
            _paymentNotificationService = paymentNotificationService;
        }

        [Authorize(Policy = "SePayWebhook")]
        [HttpPost("verify-payment-of-snapshot")]
        public async Task<IActionResult> VerifyPaymenForSnapshottWebhook(SepayWebhookRequest request)
        {
            _logger.LogInformation(
                "[SEPAY][RECEIVED] Ref={ReferenceCode}, Code={Code}, Amount={Amount}, Gateway={Gateway}",
                request.ReferenceCode,
                request.Code,
                request.TransferAmount,
                request.Gateway);

            var result = await _orderService.CreatePrepaidOnlineOrderFromSepayWebhookAsync(request);

            await _paymentNotificationService.NotifyPaymentResultAsync(result, request);

            return Ok(new
            {
                success = true
            });
        }
    }
}
