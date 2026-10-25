using Microsoft.AspNetCore.SignalR;
using Techstore.API.Hubs;
using TechStore.Common.Constants;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Order;
using TechStore.Model.DTOs.Payment;
using TechStoreAPI.Controllers;

namespace TechStoreAPI.Hubs
{

    public interface IPaymentNotificationService
    {
        Task NotifyPaymentSuccessAsync(string snapshotId, decimal amount, string message);

        Task NotifyPaymentFailedAsync(string snapshotId, decimal amount, string message);

        Task NotifyPaymentResultAsync(ServiceResult<CreatePrePayOnlineOrderResult> result, SepayWebhookRequest request);
    }

    public class SignalRPaymentNotificationService : IPaymentNotificationService
    {
        private readonly IHubContext<PaymentHub> _hubContext;
        private readonly ILogger<PaymentWebhookController> _logger;

        public SignalRPaymentNotificationService(
            IHubContext<PaymentHub> hubContext,
            ILogger<PaymentWebhookController> logger
            )
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyPaymentSuccessAsync(string snapshotId, decimal amount, string message)
        {
            await _hubContext.Clients.Group(snapshotId)
                .SendAsync("PaymentSuccess", new
                {
                    paymentId = snapshotId,
                    amount,
                    message
                });
        }

        public async Task NotifyPaymentFailedAsync(string snapshotId, decimal amount, string message)
        {
            await _hubContext.Clients.Group(snapshotId)
                .SendAsync("PaymentFailed", new
                {
                    paymentId = snapshotId,
                    amount,
                    message
                });
        }

        public async Task NotifyPaymentResultAsync(ServiceResult<CreatePrePayOnlineOrderResult> result, SepayWebhookRequest request)
        {
            if (result.Data != null)
            {
                _logger.LogInformation(
                        "[SEPAY][SUCCESS] SnapshotId={SnapshotId}, Amount={Amount}, Message={Message}",
                        result.Data.PaymentSnapshotId,
                        result.Data.Amount,
                        result.Message);

                await _hubContext.Clients
                    .Group(result.Data.PaymentSnapshotId)
                    .SendAsync("PaymentSuccess", new CreatePrePayOnlineOrderResult
                    {
                        PaymentSnapshotId = result.Data.PaymentSnapshotId,
                        Amount = result.Data.Amount,
                    });
            }
            else
            {
                _logger.LogWarning(
                    "[SEPAY][NO_SNAPSHOT] Ref={ReferenceCode}, Code={Code}, Amount={Amount}",
                    request.ReferenceCode,
                    request.Code,
                    request.TransferAmount);

                await _hubContext.Clients
                        .Group(request.Code)
                        .SendAsync("PaymentFailed", new
                        {
                            PaymentSnapshotId = request.Code,
                            amount = request.TransferAmount,
                            message = result.Message
                        });
            }
        }
    }
}
