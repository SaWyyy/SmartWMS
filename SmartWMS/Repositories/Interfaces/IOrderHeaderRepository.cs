using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.ResponseDTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IOrderHeaderRepository
{
    Task<OrderHeader> Add(OrderHeaderDto dto);
    Task<IEnumerable<OrderHeaderDto>> GetAll();
    Task<IEnumerable<AllOrdersInfoDto>> GetAllWithDetails();
    Task<OrderHeaderDto> Get(int id);
    Task<OrderHeader> Delete(int id);
    Task<OrderHeader> Update(int id, OrderHeaderDto dto);
    Task<bool> CheckOrderDetailsForOrderHeader(int id);
}