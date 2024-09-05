using SmartWMS.Models;

namespace SmartWMS.Repositories;

public interface ICategoryRepository
{
    Task<Category> AddCategory(CategoryDto dto);
    Task<CategoryDto> GetCategory(int id);
    Task<IEnumerable<CategoryDto>> GetAll();
    Task<Category> Delete(int id);

    Task<Category> Update(int id, CategoryDto dto);
}