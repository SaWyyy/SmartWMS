using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<Category> AddCategory(CategoryDto dto);
    Task<CategoryDto> GetCategory(int id);
    Task<IEnumerable<CategoryDto>> GetAll();
    Task<IEnumerable<Category>> GetWithSubcategories();
    Task<Category> Delete(int id);

    Task<Category> Update(int id, CategoryDto dto);
}