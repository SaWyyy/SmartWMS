using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IRackRepository
{
    Task<Rack> Add(RackDto dto);
    Task<IEnumerable<RackDto>> GetAll();
    Task<RackDto> Get(int id);
    Task<Rack> Delete(int id);
    Task<Rack> Update(int id, RackDto dto);
    Task<IEnumerable<LanesRacksDto>> GetAllLanesRacks(int laneId);
}