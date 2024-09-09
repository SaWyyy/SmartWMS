using SmartWMS.Models;

namespace SmartWMS.Repositories;

public interface IAlertRepository
{
    Task<Alert> Add(AlertDto dto);
    Task<IEnumerable<AlertDto>> GetAll();
    Task<AlertDto> Get(int id);
    Task<Alert> Update(int id, AlertDto dto);
    Task<Alert> Delete(int id);
}