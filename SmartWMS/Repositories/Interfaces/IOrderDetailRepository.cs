using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IOrderDetailRepository
{
    Task<OrderDetail> Add(OrderDetailDto dto);
    Task<IEnumerable<OrderDetailDto>> GetAll();
    Task<OrderDetailDto> Get(int id);
    Task<OrderDetail> Update(int id, OrderDetailDto dto);
    Task<OrderDetail> Delete(int id);
}