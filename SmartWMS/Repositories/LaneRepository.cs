using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.ResponseDTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Repositories;

public class LaneRepository : ILaneRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public LaneRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }
    
    public async Task<Lane> Add(LaneDto dto)
    {
        dto.LaneId = null;
        var lane = _mapper.Map<Lane>(dto);
        await _dbContext.Lanes.AddAsync(lane);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return lane;

        throw new SmartWMSExceptionHandler("Error has occured while adding lane");
    }

    public async Task<IEnumerable<LaneDto>> GetAll()
    {
        var lanes = await _dbContext.Lanes.ToListAsync();
        return _mapper.Map<List<LaneDto>>(lanes);
    }

    public async Task<IEnumerable<LaneRacksShelvesDto>> GetAllWithRacksShelves()
    {
        var lanes = await _dbContext.Lanes
            .Include(r => r.Racks)
            .ThenInclude(s => s.Shelves)
            .Select(d => new LaneRacksShelvesDto
            {
                LaneId = d.LaneId,
                LaneCode = d.LaneCode,
                Racks = d.Racks.Select(x => new RackShelvesDto
                {
                    RackId = x.RackId,
                    RackNumber = x.RackNumber,
                    Shelves = x.Shelves.Select(y => new ShelfDto
                    {
                        CurrentQuant = y.CurrentQuant,
                        Level = y.Level,
                        MaxQuant = y.MaxQuant,
                        ProductsProductId = y.ProductsProductId,
                        RacksRackId = y.RacksRackId,
                        ShelfId = y.ShelfId
                    }).ToList()
                }).ToList()
            }).ToListAsync();

        return lanes;
    }

    public async Task<LaneDto> Get(int id)
    {
        var lane = await _dbContext.Lanes.FirstOrDefaultAsync(x => x.LaneId == id);

        if (lane is null)
            throw new SmartWMSExceptionHandler("Lane with specified id hasn't been found");

        return _mapper.Map<LaneDto>(lane);
    }

    public async Task<Lane> Delete(int id)
    {
        var lane = await _dbContext.Lanes.FirstOrDefaultAsync(x => x.LaneId == id);

        if (lane is null)
            throw new SmartWMSExceptionHandler("Lane with specified id hasn't been found");

        _dbContext.Lanes.Remove(lane);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return lane;

        throw new SmartWMSExceptionHandler("Error has occured while deleting lane");
    }

    public async Task<Lane> Update(int id, LaneDto dto)
    {
        var lane = await _dbContext.Lanes.FirstOrDefaultAsync(x => x.LaneId == id);

        if (lane is null)
            throw new SmartWMSExceptionHandler("Lane with specified id hasn't been found");

        lane.LaneCode = dto.LaneCode;
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return lane;

        throw new SmartWMSExceptionHandler("Error has occured while editing lane");
    }
}