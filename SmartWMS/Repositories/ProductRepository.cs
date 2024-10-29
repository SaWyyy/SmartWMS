using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProductRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }
    
    public async Task<Product> Add(ProductDto dto)
    {
        var warehouse = await _dbContext.Warehouses.FirstOrDefaultAsync(x => x.WarehouseId == 1);

        if (warehouse is null)
            throw new SmartWMSExceptionHandler("Warehouse hasn't been found");

        var subcategory =
            await _dbContext.Subcategories.FirstOrDefaultAsync(x => 
                x.SubcategoryId == dto.SubcategoriesSubcategoryId);

        if (subcategory is null)
            throw new SmartWMSExceptionHandler("Subcategory hasn't been found");

        var product = new Product
        {
            ProductName = dto.ProductName,
            ProductDescription = dto.ProductDescription,
            Price = dto.Price,
            WarehousesWarehouseId = 1,
            Quantity = dto.Quantity,
            Barcode = dto.Barcode,
            SubcategoriesSubcategoryId = dto.SubcategoriesSubcategoryId
        };

        await _dbContext.Products.AddAsync(product);

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return product;

        throw new SmartWMSExceptionHandler("Error has occured while adding product");
    }

    public async Task<IEnumerable<ProductDto>> GetAll()
    {
        var result = await _dbContext.Products.ToListAsync();
        return _mapper.Map<List<ProductDto>>(result);
    }

    public async Task<ProductDto> Get(int id)
    {
        var result = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);

        if (result is null)
            throw new SmartWMSExceptionHandler("Product hasn't been found");

        return _mapper.Map<ProductDto>(result);
    }

    public async Task<Product> Update(int id, ProductDto dto)
    {
        var subcategory =
            await _dbContext.Subcategories.FirstOrDefaultAsync(x => 
                x.SubcategoryId == dto.SubcategoriesSubcategoryId);

        if (subcategory is null)
            throw new SmartWMSExceptionHandler("Subcategory hasn't been found");
        
        var result = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);

        if (result is null)
            throw new SmartWMSExceptionHandler("Product hasn't been found");

        result.ProductName = dto.ProductName;
        result.ProductDescription = dto.ProductDescription;
        result.Price = dto.Price;
        result.Quantity = dto.Quantity;
        result.Barcode = dto.Barcode;
        result.SubcategoriesSubcategoryId = dto.SubcategoriesSubcategoryId;

        var result2 = await _dbContext.SaveChangesAsync();

        if (result2 > 0)
            return result;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to products table");
    }

    public async Task<Product> Delete(int id)
    {
        var product = await _dbContext.Products
            .Include(x => x.OrderDetails)
            .FirstOrDefaultAsync(x => x.ProductId == id);

        if (product is null)
            throw new SmartWMSExceptionHandler("Product hasn't been found");

        if (product.OrderDetails.Any())
            throw new ConflictException("There are order details assigned to this product");

        product.IsDeleted = true;

        var result = await _dbContext.SaveChangesAsync();

        if (result <= 0)
            throw new SmartWMSExceptionHandler("Error has occured while saving changes to products table");

        return product;
    }
}