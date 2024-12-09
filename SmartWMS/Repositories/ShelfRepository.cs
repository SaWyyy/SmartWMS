using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.ResponseDTOs;
using SmartWMS.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace SmartWMS.Repositories;


public class ShelfRepository : IShelfRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public ShelfRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }

    public async Task<Shelf> AddShelf(ShelfDto dto)
    {
        dto.ShelfId = null;
        dto.CurrentQuant = 0;
        var rack = await _dbContext.Racks.FirstOrDefaultAsync(x => x.RackId == dto.RacksRackId);

        if (rack is null)
            throw new SmartWMSExceptionHandler("Rack with specified id hasn't been found");
        
        var shelf = _mapper.Map<Shelf>(dto); 
        
        await _dbContext.Shelves.AddAsync(shelf);

        var result = await _dbContext.SaveChangesAsync();
        
        
        if (result > 0)
            return shelf;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to shelf table");
    }

    public async Task<OrderShelvesAllocation> SaveAllocation(OrderShelvesAllocation dto)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.ProductId == dto.ProductId);

        if (product is null)
            throw new SmartWMSExceptionHandler("Order detail does not exist");

        var shelf = await _dbContext.Shelves
            .FirstOrDefaultAsync(x => x.ShelfId == dto.ShelfId);

        if (shelf is null)
            throw new SmartWMSExceptionHandler("Shelf does not exist");

        await _dbContext.OrderShelvesAllocations.AddAsync(dto);
        
        var result = await _dbContext.SaveChangesAsync();
        
        
        if (result > 0)
            return dto;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to order shelf allocation table");
    }

    public async Task<IEnumerable<ShelfDto>> GetAll()
    {
        var shelves = await _dbContext.Shelves.ToListAsync();
        var shelfDtos = _mapper.Map<List<ShelfDto>>(shelves);

        return shelfDtos;
    }

    public async Task<IEnumerable<ShelfRackDto>> GetAllWithRackLanes()
    {
        var shelves = await _dbContext.Shelves
            .Include(r => r.RackRack)
            .ThenInclude(l => l.LaneLane)
            .Select(x => new ShelfRackDto()
            {
                ShelfId = x.ShelfId,
                CurrentQuant = x.CurrentQuant,
                MaxQuant = x.MaxQuant,
                Level = x.Level,
                ProductId = x.ProductsProductId,
                RackLane = new RackLaneDto
                {
                    RackId = x.RackRack.RackId,
                    RackNumber = x.RackRack.RackNumber,
                    Lane = new LaneDto
                    {
                        LaneId = x.RackRack.LaneLane.LaneId,
                        LaneCode = x.RackRack.LaneLane.LaneCode
                    }
                }
            }).ToListAsync();

        return shelves;
    }

    public async Task<ShelfDto> Get(int id)
    {
        var shelf = await _dbContext.Shelves.FirstOrDefaultAsync(r => r.ShelfId == id);

        if (shelf is null)
            throw new SmartWMSExceptionHandler("Shelf with specified id hasn't been found");

        var result = _mapper.Map<ShelfDto>(shelf);
        return result;
    }

    public async Task<IEnumerable<OrderShelvesAllocation>> GetAllocationsByProduct(int productId)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.ProductId == productId);
        if (product is null)
            throw new SmartWMSExceptionHandler("Product does not exist");

        var result = await _dbContext.OrderShelvesAllocations
            .Where(x => x.ProductId == productId)
            .ToListAsync();
        return result;
    }

    public async Task<Shelf> Delete(int id)
    {
        var shelf = await _dbContext.Shelves
            .Include(x => x.ProductsProduct)
            .FirstOrDefaultAsync(r => r.ShelfId == id);

        if (shelf is null)
            throw new SmartWMSExceptionHandler("Shelf with specified id hasn't been found");

        if (shelf.ProductsProduct is not null)
            throw new ConflictException("Product is assigned to shelf");

        _dbContext.Shelves.Remove(shelf);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return shelf;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to shelf table");
    }

    public async Task<IEnumerable<OrderShelvesAllocation>> DeleteAllocationsByProduct(int productId)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.ProductId == productId);
        if (product is null)
            throw new SmartWMSExceptionHandler("Product does not exist");
        
        var allocations = await _dbContext.OrderShelvesAllocations
            .Where(x => x.ProductId == productId)
            .ToListAsync();

        _dbContext.OrderShelvesAllocations.RemoveRange(allocations);

        int result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return allocations;

        throw new SmartWMSExceptionHandler("Error has occured while deleting allocations");
    }

    public async Task<Shelf> Update(int id, ShelfDto dto)
    {
        var shelf = await _dbContext.Shelves.FirstOrDefaultAsync(r => r.ShelfId == id);

        if (shelf is null)
            throw new SmartWMSExceptionHandler("Shelf with specified id hasn't been found");

        if (dto.CurrentQuant > dto.MaxQuant)
            throw new SmartWMSExceptionHandler("Current quantity cannot exceed max quantity");
        
        shelf.ProductsProductId = dto.ProductsProductId;
        shelf.CurrentQuant = dto.CurrentQuant;
        shelf.MaxQuant = dto.MaxQuant;
        shelf.Level = dto.Level;

        var result = await _dbContext.SaveChangesAsync();
        
        if (result > 0)
            return shelf;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to shelf table");
    }

    public async Task<IEnumerable<RacksLevelsDto>> GetAllRacksLevels(int rackId)
    {
        var rack = await _dbContext.Racks.FirstOrDefaultAsync(rack => rack.RackId == rackId);
        
        if (rack is null)
            throw new SmartWMSExceptionHandler("Cannot fetch rack's levels because provided id doesn't exist");
        
        var levels = await _dbContext.Shelves.Where(shelf => shelf.RacksRackId == rackId).ToListAsync();
        return _mapper.Map<List<RacksLevelsDto>>(levels);
    }
}