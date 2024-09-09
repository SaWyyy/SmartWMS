using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IProductDetailRepository
{
    Task<ProductDetail> Add(ProductDetailDto dto);
    Task<IEnumerable<ProductDetailDto>> GetAll();
    Task<ProductDetailDto> Get(int id);
    Task<ProductDetail> Delete(int id);
    Task<ProductDetail> Update(int id, ProductDetailDto dto);
}