using SmartWMS.Models;

namespace SmartWMS.Repositories;

public interface IShelfRepository
{
    Task<Shelf?> AddShelf(ShelfDto dto);

    Task<IEnumerable<ShelfDto>> GetAll();

    Task<ShelfDto?> Get(int id);
    Task<Shelf?> Delete(int id);
    Task<Shelf?> Update(int id, ShelfDto dto);
}