namespace SmartWMS.Services.Interfaces;

public interface IOrderCancellationService
{
    Task CancelOrder(int orderHeaderId);
}