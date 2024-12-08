using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.CreateOrderDtos;
using SmartWMSTests.IntegrationTests.Configuration;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace SmartWMSTests.IntegrationTests;

[Collection("Non-Parallel tests")]
public class ProductAssignmentServiceTest : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private readonly SmartwmsDbContext _dbContext;
    private readonly IServiceScope _scope;
    private readonly ITestOutputHelper _helper;
    
    public ProductAssignmentServiceTest(IntegrationTestWebAppFactory factory, ITestOutputHelper helper) : base(factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
        _helper = helper;
    }

    private async Task<int> SeedShelves(int laneCode)
    {
        var lane = new Lane
        {
            LaneCode = "D" + laneCode
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
            RacksRackId = rack.RackId,
            MaxQuant = 10,
            CurrentQuant = 10,
            Level = LevelType.P0
        };

        await _dbContext.Shelves.AddAsync(shelf);
        await _dbContext.SaveChangesAsync();

        return shelf.ShelfId;
    }

    private async Task<int> SeedSubcategories(string subcategoryName)
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
    public async Task CreateAndAssignProductToShelves_ShouldReturnOk()
    {
        // Arrange
        var subcategoryId = await SeedSubcategories("create method");
        var shelfId = await SeedShelves(1);

        var createDto = new CreateProductAsssignShelfDto
        {
            ProductDto = new ProductDto
            {
                ProductName = "Test product assign service",
                Barcode = "12345678",
                Price = 10,
                Quantity = 2,
                SubcategoriesSubcategoryId = subcategoryId
            },
            Shelves = new List<ShelfDto>
            {
                new ShelfDto
                {
                    ShelfId = shelfId,
                    MaxQuant = 10,
                    CurrentQuant = 2,
                    Level = LevelType.P0,
                }
            }
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/Product/createAndAssignToShelves", createDto);
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be("Product created and assigned successfully");
    }
    
    [Fact]
    public async Task AssignProductForDelivery_ShouldReturnOk()
    {
        // Arrange
        var subcategoryId = await SeedSubcategories("delivery method");
        var shelfId = await SeedShelves(2);

        var product = new Product
        {
            ProductName = "Test product assign service",
            Barcode = "12345678",
            Price = 10,
            Quantity = 2,
            SubcategoriesSubcategoryId = subcategoryId,
            WarehousesWarehouseId = 1
        };

        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        
        var createDto = new CreateProductAsssignShelfDto
        {
            ProductDto = new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = "Test product assign service",
                Barcode = "12345678",
                Price = 10,
                Quantity = 2,
                SubcategoriesSubcategoryId = subcategoryId
            },
            Shelves = new List<ShelfDto>
            {
                new ShelfDto
                {
                    ShelfId = shelfId,
                    MaxQuant = 10,
                    CurrentQuant = 2,
                    Level = LevelType.P0,
                }
            }
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/Product/takeDeliveryAndDistribute", createDto);
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        _helper.WriteLine(content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be("Product assigned for delivery");
    }
}