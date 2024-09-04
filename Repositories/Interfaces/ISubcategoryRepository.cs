using SmartWMS.Models;

namespace SmartWMS.Repositories;

public interface ISubcategoryRepository
{
    Task<SubcategoryDto> Add(SubcategoryDto dto);
    Task<SubcategoryDto> Get(int id);
    Task<IEnumerable<SubcategoryDto>> GetAll();
    Task<Subcategory> Delete(int id);
    Task<Subcategory> Update(int id, SubcategoryDto dto);
}