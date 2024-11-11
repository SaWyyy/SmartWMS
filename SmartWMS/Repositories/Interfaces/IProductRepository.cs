using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product> Add(ProductDto dto);
    Task<IEnumerable<ProductDto>> GetAll();
    Task<ProductDto> Get(int id);
    Task<ProductShelfDto> GetWithShelves(int id);
    Task<Product> Update(int id, ProductDto dto);
    Task<Product> Delete(int id);
}