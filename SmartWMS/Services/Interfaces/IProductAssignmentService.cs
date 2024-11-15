using SmartWMS.Models.DTOs;

namespace SmartWMS.Services.Interfaces;

public interface IProductAssignmentService
{
    Task CreateAndAssignProductToShelves(CreateProductAsssignShelfDto dto);
    Task AssignProductForDelivery(CreateProductAsssignShelfDto dto);
}