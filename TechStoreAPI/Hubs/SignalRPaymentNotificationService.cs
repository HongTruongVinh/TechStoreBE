using Microsoft.AspNetCore.SignalR;
using Techstore.API.Hubs;
using TechStore.Common.Constants;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Payment;
using TechStoreAPI.Controllers;

namespace TechStoreAPI.Hubs
{

    public interface IPaymentNotificationService
    {
        Task NotifyPaymentSuccessAsync(string snapshotId, decimal amount, string message);

        Task NotifyPaymentFailedAsync(string snapshotId, decimal amount, string message);

        Task NotifyPaymentResultAsync(ServiceResult<VerifyResult> result, SepayWebhookRequest request);
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

        public async Task NotifyPaymentResultAsync(ServiceResult<VerifyResult> result, SepayWebhookRequest request)
        {
            if (result.Data != null)
            {
                if (result.IsSuccess)
                {
                    _logger.LogInformation(
                        "[SEPAY][SUCCESS] SnapshotId={SnapshotId}, Amount={Amount}, Message={Message}",
                        result.Data.SnapshotId,
                        result.Data.Amount,
                        result.Data.Message);

                    await _hubContext.Clients
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
                    _logger.LogWarning(
                        "[SEPAY][FAILED] SnapshotId={SnapshotId}, Amount={Amount}, Message={Message}",
                        result.Data.SnapshotId,
                        result.Data.Amount,
                        result.Data.Message);

                    await _hubContext.Clients
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
                _logger.LogWarning(
                    "[SEPAY][NO_SNAPSHOT] Ref={ReferenceCode}, Code={Code}, Amount={Amount}",
                    request.ReferenceCode,
                    request.Code,
                    request.TransferAmount);

                await _hubContext.Clients
                        .Group(request.Code)
                        .SendAsync("PaymentFailed", new
                        {
                            paymentId = request.Code,
                            amount = request.TransferAmount,
                            message = Messenger.SystemError
                        });
            }
        }
    }
}
