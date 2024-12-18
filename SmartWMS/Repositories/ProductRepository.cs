using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.ResponseDTOs;
using SmartWMS.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

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

    public async Task<IEnumerable<ProductDto>> GetAllWithQuantityGtZero()
    {
        var result = await _dbContext.Products
            .Where(x => x.Quantity > 0)
            .ToListAsync();

        return _mapper.Map<List<ProductDto>>(result);
    }

    public async Task<ProductDto> Get(int id)
    {
        var result = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);

        if (result is null)
            throw new SmartWMSExceptionHandler("Product hasn't been found");

        return _mapper.Map<ProductDto>(result);
    }

    public async Task<ProductDto> GetByBarcode(string barcode)
    {
        var result = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Barcode.Equals(barcode));

        if (result is null)
            throw new SmartWMSExceptionHandler("Product with specified barcode hasn't been found");

        return _mapper.Map<ProductDto>(result);
    }

    public async Task<ProductShelfDto> GetWithShelves(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);
        
        if (product is null)
            throw new SmartWMSExceptionHandler("Product with specified id hasn't been found");
        
        var result = await _dbContext.Products
            .Include(s => s.Shelves)
            .ThenInclude(r => r.RackRack)
            .ThenInclude(l => l.LaneLane)
            .Select(x => new ProductShelfDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductDescription = x.ProductDescription,
                Barcode = x.Barcode,
                Price = x.Price,
                Quantity = x.Quantity,
                Shelves = x.Shelves.Select(y => new ShelfRackDto
                {
                    ShelfId = y.ShelfId,
                    Level = y.Level,
                    MaxQuant = y.MaxQuant,
                    CurrentQuant = y.CurrentQuant,
                    RackLane = new RackLaneDto
                    {
                        RackId = y.RackRack.RackId,
                        RackNumber = y.RackRack.RackNumber,
                        Lane = new LaneDto
                        {
                            LaneId = y.RackRack.LaneLane.LaneId,
                            LaneCode = y.RackRack.LaneLane.LaneCode
                        }
                    }
                }).ToList()
            }).FirstOrDefaultAsync(x => x.ProductId == id);

        return result!;
    }

    public async Task<IEnumerable<ProductShelfDto>> GetAllWithShelves()
    {
        var result = await _dbContext.Products
            .Include(s => s.SubcategoriesSubcategory)
            .Include(s => s.Shelves)
            .ThenInclude(r => r.RackRack)
            .ThenInclude(l => l.LaneLane)
            .Select(x => new ProductShelfDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductDescription = x.ProductDescription,
                Barcode = x.Barcode,
                Price = x.Price,
                Quantity = x.Quantity,
                SubcategoryId = x.SubcategoriesSubcategoryId,
                Shelves = x.Shelves.Select(y => new ShelfRackDto
                {
                    ShelfId = y.ShelfId,
                    Level = y.Level,
                    MaxQuant = y.MaxQuant,
                    CurrentQuant = y.CurrentQuant,
                    ProductId = y.ProductsProductId,
                    RackLane = new RackLaneDto
                    {
                        RackId = y.RackRack.RackId,
                        RackNumber = y.RackRack.RackNumber,
                        Lane = new LaneDto
                        {
                            LaneId = y.RackRack.LaneLane.LaneId,
                            LaneCode = y.RackRack.LaneLane.LaneCode
                        }
                    }
                }).ToList()
            }).ToListAsync();

        return result;
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

    public async Task<Product> UpdateQuantity(ProductDto dto)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == dto.ProductId);

        if (product is null)
            throw new SmartWMSExceptionHandler("Product with specified id does not exist");

        product.Quantity += dto.Quantity;

        await _dbContext.SaveChangesAsync();

        return product;
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
        
        var assignedShelves = await _dbContext.Shelves
            .Where(x => x.ProductsProductId == id)
            .ToListAsync();

        foreach (var shelf in assignedShelves)
        {
            shelf.ProductsProductId = null;
            shelf.CurrentQuant = 0;
            shelf.MaxQuant = 0;
        }

        product.IsDeleted = true;

        var result = await _dbContext.SaveChangesAsync();

        if (result <= 0)
            throw new SmartWMSExceptionHandler("Error has occured while saving changes to products table");

        return product;
    }
}