using Microsoft.AspNetCore.SignalR;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services.Interfaces;
using SmartWMS.SignalR;

namespace SmartWMS.Services;

public class InventoryStatusService : IInventoryStatusService
{
    private readonly IProductRepository _repository;
    private readonly IHubContext<NotificationHub> _hubContext;

    public InventoryStatusService(IProductRepository repository, IHubContext<NotificationHub> hubContext)
    {
        this._repository = repository;
        this._hubContext = hubContext;
    }
    
    public async Task<bool> CheckInventory()
    {
        var products = (await _repository.GetAll()).ToList();
        if (!products.Any())
            return false;
        
        var lowQuantityProducts = products
            .Where(productDto => productDto.Quantity < 3)
            .Select(productDto => productDto.ProductName)
            .ToList();

        if (lowQuantityProducts.Any())
        {
            var result = string.Join(", ", lowQuantityProducts);
            await _hubContext.Clients.Groups("Employee").SendAsync($"Low quantity of: {result}");
            
            return true;
        }
        
        return false;
    }
}