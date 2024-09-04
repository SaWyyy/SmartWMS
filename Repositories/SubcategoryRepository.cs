using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Models;

namespace SmartWMS.Repositories;

public class SubcategoryRepository: ISubcategoryRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public SubcategoryRepository(SmartwmsDbContext doContext, IMapper mapper)
    {
        _dbContext = doContext;
        _mapper = mapper;
    }

    public async Task<SubcategoryDto> Add(SubcategoryDto dto)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(r => r.CategoryId == dto.CategoriesCategoryId);
        if (category is null)
            throw new Exception("Wrong category's id has been passed while creating new subcategory");

        await _dbContext.AddAsync(_mapper.Map<Subcategory>(dto));
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return dto;

        throw new Exception("Error has occured while adding country");
    }

    public async Task<SubcategoryDto> Get(int id)
    {
        var result = await _dbContext.Subcategories.FirstOrDefaultAsync(r => r.SubcategoryId == id);

        if (result is null)
            throw new Exception("Subcategory with specified id hasn't been found");

        return _mapper.Map<SubcategoryDto>(result);
    }

    public async Task<IEnumerable<SubcategoryDto>> GetAll()
    {
        return _mapper.Map<IEnumerable<SubcategoryDto>>(await _dbContext.Subcategories.ToListAsync());
    }
    
    public async Task<Subcategory> Delete(int id)
    {
        var subcategory = await _dbContext.Subcategories.FirstOrDefaultAsync(r => r.SubcategoryId == id);

        if (subcategory is null)
            throw new Exception("Subcategory with specified id hasn't been found");

        _dbContext.Subcategories.Remove(subcategory);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return subcategory;

        throw new Exception("Error has occured while saving changes to database");
    }

    public async Task<Subcategory> Update(int id, SubcategoryDto dto)
    {
        var subcategory = await _dbContext.Subcategories.FirstOrDefaultAsync(r => r.SubcategoryId == id);

        if (subcategory is null)
            throw new Exception("Subcategory with specified id hasn't been found");

        var categoryId = await _dbContext.Categories.FirstOrDefaultAsync(r => r.CategoryId == dto.CategoriesCategoryId);

        if (categoryId is null)
            throw new Exception("Cannot assign new CategoryId because provided one doesn't exist");

        subcategory.CategoriesCategoryId = dto.CategoriesCategoryId;
        subcategory.SubcategoryName = dto.SubcategoryName;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return subcategory;
        throw new Exception("Error has occured while saving changes");
    }
}