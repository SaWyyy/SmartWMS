using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace SmartWMS.SignalR;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;
        
        if (userRole is "employee")
            await Groups.AddToGroupAsync(Context.ConnectionId, "employee");
        
        await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId}: has joined");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "employee");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", $"{message}");
    }
    
    
}