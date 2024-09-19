using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
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
        var shelf = _mapper.Map<Shelf>(dto); 
        
        await _dbContext.Shelves.AddAsync(shelf);

        var result = await _dbContext.SaveChangesAsync();
        
        
        if (result > 0)
            return shelf;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to shelf table");
    }

    public async Task<IEnumerable<ShelfDto>> GetAll()
    {
        var shelves = await _dbContext.Shelves.ToListAsync();
        var shelfDtos = _mapper.Map<List<ShelfDto>>(shelves);

        return shelfDtos;
    }

    public async Task<ShelfDto> Get(int id)
    {
        var shelf = await _dbContext.Shelves.FirstOrDefaultAsync(r => r.ShelfId == id);

        if (shelf is null)
            throw new SmartWMSExceptionHandler("Shelf with specified id hasn't been found");

        var result = _mapper.Map<ShelfDto>(shelf);
        return result;
    }

    public async Task<Shelf> Delete(int id)
    {
        var shelf = await _dbContext.Shelves.FirstOrDefaultAsync(r => r.ShelfId == id);

        if (shelf is null)
            throw new SmartWMSExceptionHandler("Shelf with specified id hasn't been found");

        _dbContext.Shelves.Remove(shelf);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return shelf;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to shelf table");
    }

    public async Task<Shelf> Update(int id, ShelfDto dto)
    {
        var shelf = await _dbContext.Shelves.FirstOrDefaultAsync(r => r.ShelfId == id);

        if (shelf is null)
            throw new SmartWMSExceptionHandler("Shelf with specified id hasn't been found");

        shelf.ProductsProductId = dto.ProductsProductId;
        shelf.CurrentQuant = dto.CurrentQuant;
        shelf.Lane = dto.Lane;
        shelf.Rack = dto.Rack;
        shelf.MaxQuant = dto.MaxQuant;
        shelf.Level = dto.Level;

        var result = await _dbContext.SaveChangesAsync();
        
        if (result > 0)
            return shelf;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to shelf table");
    }
}