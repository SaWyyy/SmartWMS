using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Repositories;

public class RackRepository : IRackRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public RackRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }
    
    public async Task<Rack> Add(RackDto dto)
    {
        dto.RackId = null;
        var lane = await _dbContext.Lanes.FirstOrDefaultAsync(x => x.LaneId == dto.LanesLaneId);

        if (lane is null)
            throw new SmartWMSExceptionHandler("Lane with specified id does not exist");

        var rack = _mapper.Map<Rack>(dto);
        await _dbContext.Racks.AddAsync(rack);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return rack;

        throw new SmartWMSExceptionHandler("Error has occured while adding rack");
    }

    public async Task<IEnumerable<RackDto>> GetAll()
    {
        var racks = await _dbContext.Racks.ToListAsync();
        return _mapper.Map<List<RackDto>>(racks);
    }

    public async Task<RackDto> Get(int id)
    {
        var rack = await _dbContext.Racks.FirstOrDefaultAsync(x => x.RackId == id);

        if (rack is null)
            throw new SmartWMSExceptionHandler("Rack with specified id hasn't been found");

        return _mapper.Map<RackDto>(rack);
    }

    public async Task<Rack> Delete(int id)
    {
        var rack = await _dbContext.Racks.FirstOrDefaultAsync(x => x.RackId == id);
        
        if (rack is null)
            throw new SmartWMSExceptionHandler("Rack with specified id hasn't been found");

        _dbContext.Racks.Remove(rack);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return rack;

        throw new SmartWMSExceptionHandler("Error has occured while deleting rack");
    }

    public async Task<Rack> Update(int id, RackDto dto)
    {
        var rack = await _dbContext.Racks.FirstOrDefaultAsync(x => x.RackId == id);
        
        if (rack is null)
            throw new SmartWMSExceptionHandler("Rack with specified id hasn't been found");

        rack.RackNumber = dto.RackNumber;
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return rack;

        throw new SmartWMSExceptionHandler("Error has occured while updating rack");
    }
    
    public async Task<IEnumerable<LanesRacksDto>> GetAllLanesRacks(int laneId)
    {
        var lane = await _dbContext.Lanes.FirstOrDefaultAsync(lane => lane.LaneId == laneId);
        
        if (lane is null)
            throw new SmartWMSExceptionHandler("Cannot fetch lane's racks because provided id doesn't exist");
        
        var racks = await _dbContext.Racks.Where(rack => rack.LanesLaneId == laneId).ToListAsync();
        return _mapper.Map<List<LanesRacksDto>>(racks);
    }
}