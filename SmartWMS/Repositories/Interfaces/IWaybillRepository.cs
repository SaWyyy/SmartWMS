using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IWaybillRepository
{
    Task<Waybill> AddWaybill(WaybillDto dto);
    Task<IEnumerable<WaybillDto>> GetAll();
    Task<WaybillDto> Get(int id);
    Task<Waybill> Delete(int id);
    Task<Waybill> Update(int id, WaybillDto dto);
}