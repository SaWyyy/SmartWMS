using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

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

    public async Task<Subcategory> Add(SubcategoryDto dto)
    {
        dto.SubcategoryId = null;
        var category = await _dbContext.Categories.FirstOrDefaultAsync(r => r.CategoryId == dto.CategoriesCategoryId);
        if (category is null)
            throw new SmartWMSExceptionHandler("Wrong category's id has been passed while creating new subcategory");

        var subcategory = _mapper.Map<Subcategory>(dto);
        await _dbContext.AddAsync(subcategory);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return subcategory;

        throw new SmartWMSExceptionHandler("Error has occured while adding subcategory");
    }

    public async Task<SubcategoryDto> Get(int id)
    {
        var result = await _dbContext.Subcategories.FirstOrDefaultAsync(r => r.SubcategoryId == id);

        if (result is null)
            throw new SmartWMSExceptionHandler("Subcategory with specified id hasn't been found");

        return _mapper.Map<SubcategoryDto>(result);
    }

    public async Task<IEnumerable<SubcategoryDto>> GetAllByCategory(int categoryId)
    {
        var subcategories = await _dbContext.Subcategories
            .Where(x => x.CategoriesCategoryId == categoryId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<SubcategoryDto>>(subcategories);
    }

    public async Task<IEnumerable<SubcategoryDto>> GetAll()
    {
        return _mapper.Map<IEnumerable<SubcategoryDto>>(await _dbContext.Subcategories.ToListAsync());
    }
    
    public async Task<Subcategory> Delete(int id)
    {
        var subcategory = await _dbContext.Subcategories
            .Include(x => x.Products)
            .FirstOrDefaultAsync(r => r.SubcategoryId == id);

        if (subcategory is null)
            throw new SmartWMSExceptionHandler("Subcategory with specified id hasn't been found");

        if (subcategory.Products.Any())
            throw new ConflictException("There are products assigned to this subcategory");

        subcategory.IsDeleted = true;
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return subcategory;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to subcategory table");
    }

    public async Task<Subcategory> Update(int id, SubcategoryDto dto)
    {
        var subcategory = await _dbContext.Subcategories.FirstOrDefaultAsync(r => r.SubcategoryId == id);

        if (subcategory is null)
            throw new SmartWMSExceptionHandler("Subcategory with specified id hasn't been found");

        var categoryId = await _dbContext.Categories.FirstOrDefaultAsync(r => r.CategoryId == dto.CategoriesCategoryId);

        if (categoryId is null)
            throw new SmartWMSExceptionHandler("Cannot assign new CategoryId because provided one doesn't exist");

        subcategory.CategoriesCategoryId = dto.CategoriesCategoryId;
        subcategory.SubcategoryName = dto.SubcategoryName;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return subcategory;
        throw new SmartWMSExceptionHandler("Error has occured while saving changes to subcategory table");
    }
}