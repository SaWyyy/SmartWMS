using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

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
        dto.CategoryId = null;
        var category = _mapper.Map<Category>(dto);
        await _dbContext.Categories.AddAsync(category);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return category;
        
        throw new SmartWMSExceptionHandler("Error has occured while saving changes to category table");
    }

    public async Task<CategoryDto> GetCategory(int id)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(r => r.CategoryId == id);
        if (category is null)
            throw new SmartWMSExceptionHandler("Category with specified id hasn't been found");
        
        var result = _mapper.Map<CategoryDto>(category);
        return result;
    }

    public async Task<IEnumerable<CategoryDto>> GetAll()
    {
        var categories = await _dbContext.Categories.ToListAsync();
        var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
        return categoriesDto;
    }

    public async Task<IEnumerable<CategorySubcategoriesDto>> GetWithSubcategories()
    {
        var categories = await _dbContext.Categories
            .Include(s => s.Subcategories)
            .Select(c => new CategorySubcategoriesDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Subcategories = c.Subcategories.Select(s => new SubcategoryDto
                {
                    SubcategoryId = s.SubcategoryId,
                    SubcategoryName = s.SubcategoryName,
                    CategoriesCategoryId = c.CategoryId
                }).ToList()
            }).ToListAsync();
        
        return categories;
    }

    public async Task<Category> Delete(int id)
    {
        var category = await _dbContext.Categories
            .Include(x => x.Subcategories)
            .FirstOrDefaultAsync(r => r.CategoryId == id);
        
        if (category is null)
            throw new SmartWMSExceptionHandler("Category with specified id hasn't been found");

        if (category.Subcategories.Any())
            throw new ConflictException("There are subcategories assigned to category");


        category.IsDeleted = true;
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return category;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to category table");
    }

    public async Task<Category> Update(int id, CategoryDto dto)
    {
        dto.CategoryId = null;
        var category = await _dbContext.Categories.FirstOrDefaultAsync(r => r.CategoryId == id);

        if (category is null)
            throw new SmartWMSExceptionHandler("Category with specified id hasn't been found");

        category.CategoryName = dto.CategoryName;
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return _mapper.Map<Category>(category);

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to category table");
    }
}