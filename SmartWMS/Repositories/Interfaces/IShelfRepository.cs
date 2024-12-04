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
    Task<IEnumerable<OrderShelvesAllocation>> GetAllocationsByProduct(int productId);
    Task<Shelf> Delete(int id);
    Task<IEnumerable<OrderShelvesAllocation>> DeleteAllocationsByProduct(int productId);
    Task<Shelf> Update(int id, ShelfDto dto);
    Task<IEnumerable<RacksLevelsDto>> GetAllRacksLevels(int rackId);
}