using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IShelfRepository
{
    Task<Shelf> AddShelf(ShelfDto dto);

    Task<IEnumerable<ShelfDto>> GetAll();

    Task<ShelfDto> Get(int id);
    Task<Shelf> Delete(int id);
    Task<Shelf> Update(int id, ShelfDto dto);
    Task<IEnumerable<RacksLevelsDto>> GetAllRacksLevels(int rackId);
}