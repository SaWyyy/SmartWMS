using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Models;

namespace SmartWMS.Repositories;

public class CategoryRepository: ICategoryRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoryRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }

    public async Task<Category> AddCategory(CategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        await _dbContext.Categories.AddAsync(category);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return category;
        
        throw new Exception("Error has occured while saving changes");
    }

    public async Task<Category> GetCategory(int id)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(r => r.CategoryId == id);
        if (category is null)
            throw new Exception("Category with specified id hasn't been found");
        
        var result = _mapper.Map<Category>(category);
        return result;
    }

    public async Task<IEnumerable<CategoryDto>> GetAll()
    {
        var categories = await _dbContext.Categories.ToListAsync();
        var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
        return categoriesDto;
    }

    public async Task<Category> Delete(int id)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(r => r.CategoryId == id);
        
        if (category is null)
            throw new Exception("Category with specified id hasn't been found");
        
        _dbContext.Categories.Remove(category);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return category;

        throw new Exception("Error has occured while saving changes to database");
    }

    public async Task<Category> Update(int id, CategoryDto dto)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(r => r.CategoryId == id);

        if (category is null)
            throw new Exception("Category with specified id hasn't been found");

        category.CategoryName = dto.CategoryName;
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return _mapper.Map<Category>(category);

        throw new Exception("Error has occured while saving changes to database");
    }
}