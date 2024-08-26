using SmartWMS.Models;

namespace SmartWMS.Repositories;

public interface IShelfRepository
{
    Task<Shelf> AddShelf(CreateShelfDto dto);

    Task<IEnumerable<ShelfDto>> GetAll();

    Task<ShelfDto> Get(int id);
    Task<Shelf> Delete(int id);
}