using Microsoft.AspNetCore.SignalR;

namespace Techstore.API.Hubs;

public class PaymentHub : Hub
{
    public async Task JoinPaymentGroup(string paymentId)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            paymentId
        );
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(
        Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}