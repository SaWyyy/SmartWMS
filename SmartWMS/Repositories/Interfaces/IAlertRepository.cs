using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IAlertRepository
{
    Task<Alert> Add(AlertDto dto);
    Task<IEnumerable<AlertDto>> GetAll();
    Task<AlertDto> Get(int id);
    Task<Alert> Update(int id, AlertDto dto);
    Task<Alert> Delete(int id);
    Task<Alert> ChangeSeen(int id);
}