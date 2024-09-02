using SmartWMS.Models;

namespace SmartWMS.Repositories;

public interface IOrderHeaderRepository
{
    Task<OrderHeader> Add(OrderHeaderDto dto);
    Task<IEnumerable<OrderHeaderDto>> GetAll();
    Task<OrderHeaderDto> Get(int id);
    Task<OrderHeader> Delete(int id);
    Task<OrderHeader> Update(int id, OrderHeaderDto dto);
}