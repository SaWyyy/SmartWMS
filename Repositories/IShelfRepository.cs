using SmartWMS.Models;

namespace SmartWMS.Repositories;

public interface IShelfRepository
{
    Task<Shelf> AddShelf(CreateShelfDto dto);
}