using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.ResponseDTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IShelfRepository
{
    Task<Shelf> AddShelf(ShelfDto dto);
    Task<OrderShelvesAllocation> SaveAllocation(OrderShelvesAllocation dto);
    Task<IEnumerable<ShelfDto>> GetAll();
    Task<IEnumerable<ShelfRackDto>> GetAllWithRackLanes();
    Task<ShelfDto> Get(int id);
    Task<IEnumerable<OrderShelvesAllocation>> GetAllocationsByProductAndTask(int productId, int taskId);
    Task<Shelf> Delete(int id);
    Task<IEnumerable<OrderShelvesAllocation>> DeleteAllocationsByProductAndTask(int productId, int taskId);
    Task<Shelf> Update(int id, ShelfDto dto);
    Task<IEnumerable<RacksLevelsDto>> GetAllRacksLevels(int rackId);
}