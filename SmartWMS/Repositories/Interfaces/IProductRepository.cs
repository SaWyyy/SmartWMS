using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.ResponseDTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product> Add(ProductDto dto);
    Task<IEnumerable<ProductDto>> GetAll();
    Task<IEnumerable<ProductDto>> GetAllWithQuantityGtZero();
    Task<ProductDto> Get(int id);
    Task<ProductDto> GetByBarcode(string barcode);
    Task<ProductShelfDto> GetWithShelves(int id);
    Task<IEnumerable<ProductShelfDto>> GetAllWithShelves();

    Task<Product> Update(int id, ProductDto dto);
    Task<Product> UpdateQuantity(ProductDto dto);
    Task<Product> Delete(int id);
}