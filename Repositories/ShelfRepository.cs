using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Models;
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

    public async Task<Shelf> AddShelf(CreateShelfDto dto) 
    {

        var shelf = _mapper.Map<Shelf>(dto); 
        
        await _dbContext.Shelves.AddAsync(shelf);

        var result = await _dbContext.SaveChangesAsync();
        

        if (result > 0)
        {
            return shelf;
        }

        return null;
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
        {
            return null;
        }

        var result = _mapper.Map<ShelfDto>(shelf);
        return result;
    }

    public async Task<Shelf> Delete(int id)
    {
        var shelf = await _dbContext.Shelves.FirstOrDefaultAsync(r => r.ShelfId == id);

        if (shelf is null)
        {
            return null;
        }

        _dbContext.Shelves.Remove(shelf);
        var result = await _dbContext.SaveChangesAsync();

        if (shelf is null)
        {
            return null;
        }

        return shelf;

    }
    
    
    
}