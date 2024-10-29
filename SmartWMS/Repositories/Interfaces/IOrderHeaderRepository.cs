using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IOrderHeaderRepository
{
    Task<OrderHeader> Add(OrderHeaderDto dto);
    Task<IEnumerable<OrderHeaderDto>> GetAll();
    Task<OrderHeaderDto> Get(int id);
    Task<OrderHeader> Delete(int id);
    Task<OrderHeader> Update(int id, OrderHeaderDto dto);
    Task<bool> CheckOrderDetailsForOrderHeader(int id);
}