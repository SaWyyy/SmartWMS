using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface ISubcategoryRepository
{
    Task<Subcategory> Add(SubcategoryDto dto);
    Task<SubcategoryDto> Get(int id);
    Task<IEnumerable<SubcategoryDto>> GetAllByCategory(int categoryId);
    Task<IEnumerable<SubcategoryDto>> GetAll();
    Task<Subcategory> Delete(int id);
    Task<Subcategory> Update(int id, SubcategoryDto dto);
}