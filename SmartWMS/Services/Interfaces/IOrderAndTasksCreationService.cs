using SmartWMS.Models.CreateOrderDtos;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Services.Interfaces;

public interface IOrderAndTasksCreationService
{
    Task CreateOrder(CreateOrderDto dto);
}