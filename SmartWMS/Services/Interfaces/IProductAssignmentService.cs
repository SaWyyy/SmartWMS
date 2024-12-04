using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.CreateOrderDtos;

namespace SmartWMS.Services.Interfaces;

public interface IProductAssignmentService
{
    Task CreateAndAssignProductToShelves(CreateProductAsssignShelfDto dto);
    Task AssignProductForDelivery(CreateProductAsssignShelfDto dto);
}