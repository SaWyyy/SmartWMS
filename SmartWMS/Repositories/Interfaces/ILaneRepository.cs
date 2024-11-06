using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface ILaneRepository
{
    Task<Lane> Add(LaneDto dto);
    Task<IEnumerable<LaneDto>> GetAll();
    Task<IEnumerable<LaneRacksShelvesDto>> GetAllWithRacksShelves();
    Task<LaneDto> Get(int id);
    Task<Lane> Delete(int id);
    Task<Lane> Update(int id, LaneDto dto);
}