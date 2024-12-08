using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
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
public class ShelfIntegrationTest : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private readonly SmartwmsDbContext _dbContext;
    private readonly IServiceScope _scope;
    private readonly ITestOutputHelper _helper;
    
    public ShelfIntegrationTest(IntegrationTestWebAppFactory factory, ITestOutputHelper helper) : base(factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
        _helper = helper;
    }

    private async Task<int> SeedProduct(string productName)
    {
        var category = new Category
        {
            CategoryName = "Test category",
        };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var subcategory = new Subcategory
        {
            SubcategoryName = "Test subcategory shelf",
            CategoriesCategoryId = category.CategoryId,
        };

        await _dbContext.Subcategories.AddAsync(subcategory);
        await _dbContext.SaveChangesAsync();
        
        var product = new Product
        {
            ProductName = "Test product " + productName,
            Barcode = "12345678",
            Price = 10,
            Quantity = 2,
            SubcategoriesSubcategoryId = subcategory.SubcategoryId,
            WarehousesWarehouseId = 1
        };

        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        return product.ProductId;
    }

    private async Task<int> SeedRack(int laneCode)
    {
        var lane = new Lane
        {
            LaneCode = "E" + laneCode
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

        return rack.RackId;
    }

    [Fact]
    public async Task AddShelf_ShouldReturnOk()
    {
        // Arrange
        var productId = await SeedProduct("shelf add");
        var rackId = await SeedRack(1);

        var shelfDto = new ShelfDto
        {
            RacksRackId = rackId,
            ProductsProductId = productId,
            CurrentQuant = 0,
            MaxQuant = 10,
            Level = LevelType.P0,
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/Shelf", shelfDto);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        // Arrange
        var productId = await SeedProduct("shelf getById");
        var rackId = await SeedRack(2);
        
        var shelf = new Shelf
        {
            RacksRackId = rackId,
            ProductsProductId = productId,
            CurrentQuant = 0,
            MaxQuant = 10,
            Level = LevelType.P0,
        };

        await _dbContext.Shelves.AddAsync(shelf);
        await _dbContext.SaveChangesAsync();

        var shelfId = shelf.ShelfId;
        
        // Act
        var response = await _client.GetAsync($"/api/Shelf/{shelfId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ShelfDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.ShelfId.Should().Be(shelfId);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Arrange
        var productId = await SeedProduct("shelf getAll");
        var rackId = await SeedRack(3);
        
        var shelf = new Shelf
        {
            RacksRackId = rackId,
            ProductsProductId = productId,
            CurrentQuant = 0,
            MaxQuant = 10,
            Level = LevelType.P0,
        };

        await _dbContext.Shelves.AddAsync(shelf);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync($"/api/Shelf");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<List<ShelfDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        // Arrange
        var productId = await SeedProduct("shelf update");
        var rackId = await SeedRack(4);
        
        var shelf = new Shelf
        {
            RacksRackId = rackId,
            ProductsProductId = productId,
            CurrentQuant = 0,
            MaxQuant = 10,
            Level = LevelType.P0,
        };

        await _dbContext.Shelves.AddAsync(shelf);
        await _dbContext.SaveChangesAsync();
        
        var shelfNew = new Shelf
        {
            RacksRackId = rackId,
            ProductsProductId = productId,
            CurrentQuant = 2,
            MaxQuant = 10,
            Level = LevelType.P0,
        };

        var shelfId = shelf.ShelfId;
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/Shelf/{shelfId}", shelfNew);
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ShelfDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.ShelfId.Should().Be(shelfId);
        responseDto.CurrentQuant.Should().Be(2);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk()
    {
        // Arrange
        var productId = await SeedProduct("shelf update");
        var rackId = await SeedRack(5);
        
        var shelf = new Shelf
        {
            RacksRackId = rackId,
            ProductsProductId = productId,
            CurrentQuant = 0,
            MaxQuant = 10,
            Level = LevelType.P0,
        };

        await _dbContext.Shelves.AddAsync(shelf);
        await _dbContext.SaveChangesAsync();

        var shelfId = shelf.ShelfId;
        
        // Act
        var response = await _client.DeleteAsync($"/api/Shelf/{shelfId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ShelfDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.ShelfId.Should().Be(shelfId);
    }
    
    [Fact]
    public async Task GetAllWithRackLanes_ShouldReturnOk()
    {
        // Arrange
        var productId = await SeedProduct("shelf getAll");
        var rackId = await SeedRack(6);
        
        var shelf = new Shelf
        {
            RacksRackId = rackId,
            ProductsProductId = productId,
            CurrentQuant = 0,
            MaxQuant = 10,
            Level = LevelType.P0,
        };

        await _dbContext.Shelves.AddAsync(shelf);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync($"/api/Shelf/withRackLane");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<List<ShelfRackDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto.Should().NotBeEmpty();
        responseDto!.All(x => !x.RackLane.Equals(null)).Should().BeTrue();
    }
    
    [Fact]
    public async Task GetAllRackLevels_ShouldReturnOk()
    {
        // Arrange
        var productId = await SeedProduct("shelf getAll");
        var rackId = await SeedRack(7);
        
        var shelf = new Shelf
        {
            RacksRackId = rackId,
            ProductsProductId = productId,
            CurrentQuant = 0,
            MaxQuant = 10,
            Level = LevelType.P0,
        };

        await _dbContext.Shelves.AddAsync(shelf);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync($"/api/Shelf/racksLevels/{rackId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<List<RacksLevelsDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto.Should().NotBeEmpty();
    }
}