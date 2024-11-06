using SmartWMS.Models.ReturnEnums;

namespace SmartWMS.Services.Interfaces;

public interface IOrderValidationService
{
    Task<OrderValidation> CheckOrderCompletion(int taskId);
}