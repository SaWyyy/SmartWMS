using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SmartWMS.SignalR;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;
        
        if (userRole is "Employee")
            await Groups.AddToGroupAsync(Context.ConnectionId, "Employee");
        
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