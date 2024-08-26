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

    public async Task<Shelf?> AddShelf(CreateShelfDto dto) 
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
    
}