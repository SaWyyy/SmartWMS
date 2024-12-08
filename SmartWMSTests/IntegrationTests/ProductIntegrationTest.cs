using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.ResponseDTOs;
using SmartWMSTests.IntegrationTests.Configuration;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace SmartWMSTests.IntegrationTests;

[Collection("Non-Parallel tests")]
public class ProductIntegrationTest : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private readonly SmartwmsDbContext _dbContext;
    private readonly IServiceScope _scope;
    private readonly ITestOutputHelper _helper;
    
    public ProductIntegrationTest(IntegrationTestWebAppFactory factory, ITestOutputHelper helper) : base(factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
        _helper = helper;
    }
    
    private async Task<int> SeedDatabase(string subcategoryName)
    {
        var category = new Category
        {
            CategoryName = "Test category",
        };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var subcategory = new Subcategory
        {
            SubcategoryName = "Test subcategory " + subcategoryName,
            CategoriesCategoryId = category.CategoryId,
        };

        await _dbContext.Subcategories.AddAsync(subcategory);
        await _dbContext.SaveChangesAsync();
        
        return subcategory.SubcategoryId;
    }

    [Fact]
    public async Task AddProduct_ShouldReturnOk()
    {
        // Arrange
        var subcategoryId = await SeedDatabase("add");

        var product = new ProductDto
        {
            ProductName = "Test product add",
            Barcode = "12345678",
            Price = 10,
            Quantity = 10,
            SubcategoriesSubcategoryId = subcategoryId
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Product", product);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        // Arrange
        var subcategoryId = await SeedDatabase("getById");

        var product = new Product
        {
            ProductName = "Test product getByID",
            Barcode = "12345678",
            Price = 10,
            Quantity = 10,
            SubcategoriesSubcategoryId = subcategoryId,
            WarehousesWarehouseId = 1
        };

        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        var productId = product.ProductId;
        
        // Act
        var response = await _client.GetAsync($"/api/Product/{productId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ProductDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto.Should().NotBeNull();
        responseDto!.ProductId.Should().Be(productId);
        responseDto.ProductName.Should().Be("Test product getByID");
    }
    
    [Fact]
    public async Task GetByBarcode_ShouldReturnOk()
    {
        // Arrange
        var subcategoryId = await SeedDatabase("getByBarcode");

        var product = new Product
        {
            ProductName = "Test product getByBarcode",
            Barcode = "12345678",
            Price = 10,
            Quantity = 10,
            SubcategoriesSubcategoryId = subcategoryId,
            WarehousesWarehouseId = 1
        };

        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        var barcode = product.Barcode;
        
        // Act
        var response = await _client.GetAsync($"/api/Product/byBarcode/{barcode}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ProductDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto.Should().NotBeNull();
        responseDto!.Barcode.Should().Be(barcode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Arrange
        var subcategoryId = await SeedDatabase("getAll");
        
        var product = new Product
        {
            ProductName = "Test product getAll",
            Barcode = "12345678",
            Price = 10,
            Quantity = 10,
            SubcategoriesSubcategoryId = subcategoryId,
            WarehousesWarehouseId = 1
        };
        
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/Product");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDtos = JsonSerializer.Deserialize<List<ProductDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDtos.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetAllWithQuantityGtZero_ShouldReturnOk()
    {
        // Arrange
        var subcategoryId = await SeedDatabase("getAllWithQuantityGtZero");
        
        var product = new Product
        {
            ProductName = "Test product getAllWithQuantityGtZero",
            Barcode = "12345678",
            Price = 10,
            Quantity = 10,
            SubcategoriesSubcategoryId = subcategoryId,
            WarehousesWarehouseId = 1
        };
        
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync($"/api/Product/quantityGtZero");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDtos = JsonSerializer.Deserialize<List<ProductDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDtos.Should().NotBeNull();
        responseDtos!.Any(x => x.Quantity > 0).Should().BeTrue();
    }

    [Fact]
    public async Task GetWithShelvesById_ShouldReturnOk()
    {
        // Arrange
        var subcategoryId = await SeedDatabase("getWithShelvesById");
        
        var product = new Product
        {
            ProductName = "Test product getWithShelvesById",
            Barcode = "12345678",
            Price = 10,
            Quantity = 10,
            SubcategoriesSubcategoryId = subcategoryId,
            WarehousesWarehouseId = 1
        };
        
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        var lane = new Lane
        {
            LaneCode = "C1"
        };

        await _dbContext.Lanes.AddAsync(lane);
        await _dbContext.SaveChangesAsync();

        var rack = new Rack
        {
            RackNumber = 1,
            LanesLaneId = lane.LaneId
        };

        await _dbContext.Racks.AddAsync(rack);
        await _dbContext.SaveChangesAsync();

        var shelf = new Shelf
        {
            ProductsProductId = product.ProductId,
            RacksRackId = rack.RackId,
            MaxQuant = 10,
            CurrentQuant = 10,
            Level = LevelType.P0
        };

        await _dbContext.Shelves.AddAsync(shelf);
        await _dbContext.SaveChangesAsync();

        var productId = product.ProductId;
        
        // Act
        var response = await _client.GetAsync($"/api/Product/withShelves/{productId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ProductShelfDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.ProductId.Should().Be(productId);
        responseDto.Shelves.ToList()[0].ShelfId.Should().Be(shelf.ShelfId);
    }
    
    [Fact]
    public async Task GetAllWithShelvesById_ShouldReturnOk()
    {
        // Arrange
        var subcategoryId = await SeedDatabase("getAllWithShelvesById");
        
        var product = new Product
        {
            ProductName = "Test product getAllWithShelvesById",
            Barcode = "12345678",
            Price = 10,
            Quantity = 10,
            SubcategoriesSubcategoryId = subcategoryId,
            WarehousesWarehouseId = 1
        };
        
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        var lane = new Lane
        {
            LaneCode = "C2"
        };

        await _dbContext.Lanes.AddAsync(lane);
        await _dbContext.SaveChangesAsync();

        var rack = new Rack
        {
            RackNumber = 1,
            LanesLaneId = lane.LaneId
        };

        await _dbContext.Racks.AddAsync(rack);
        await _dbContext.SaveChangesAsync();

        var shelf = new Shelf
        {
            ProductsProductId = product.ProductId,
            RacksRackId = rack.RackId,
            MaxQuant = 10,
            CurrentQuant = 10,
            Level = LevelType.P0
        };

        await _dbContext.Shelves.AddAsync(shelf);
        await _dbContext.SaveChangesAsync();

        var productId = product.ProductId;
        
        // Act
        var response = await _client.GetAsync("/api/Product/withShelves");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<List<ProductShelfDto>>(content, customJsonOptions);
        var shelves = responseDto!.Select(x => x.Shelves).ToList();
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto.Should().NotBeEmpty();
        shelves.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        // Arrange
        var subcategoryId = await SeedDatabase("update");
        
        var product = new Product
        {
            ProductName = "Test product update",
            Barcode = "12345678",
            Price = 10,
            Quantity = 10,
            SubcategoriesSubcategoryId = subcategoryId,
            WarehousesWarehouseId = 1
        };
        
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        var productId = product.ProductId;
        
        var productNew = new ProductDto
        {
            ProductName = "Test product update updated",
            Barcode = "12345678",
            Price = 10,
            Quantity = 10,
            SubcategoriesSubcategoryId = subcategoryId
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/Product/{productId}", productNew);
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be($"Product with id: {productId} has been updated");
    }

    [Fact]
    public async Task Delete_ShouldReturnOk()
    {
        // Arrange
        var subcategoryId = await SeedDatabase("delete");
        
        var product = new Product
        {
            ProductName = "Test product delete",
            Barcode = "12345678",
            Price = 10,
            Quantity = 10,
            SubcategoriesSubcategoryId = subcategoryId,
            WarehousesWarehouseId = 1
        };
        
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        var productId = product.ProductId;
        
        // Act
        var response = await _client.DeleteAsync($"/api/Product/{productId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be($"Product with id: {productId} has been deleted");
    }
}