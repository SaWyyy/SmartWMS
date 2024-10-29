using SmartWMS.Models.ReturnEnums;

namespace SmartWMS.Services.Interfaces;

public interface IOrderValidationService
{
    Task<OrderValidation> CheckOrderCompletion(int taskId);
    Task<OrderValidation> CheckTasksForOrderDetail(int orderDetailId);
    Task<bool> CheckOrderDetailsForOrderHeader(int orderHeaderId);
}