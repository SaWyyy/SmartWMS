namespace SmartWMS.Services.Interfaces;

public interface IInventoryStatusService
{
    Task<bool> CheckInventory();
}