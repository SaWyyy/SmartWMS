using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMSTests.IntegrationTests.Configuration;
using Task = System.Threading.Tasks.Task;

namespace SmartWMSTests.IntegrationTests;

[Collection("Non-Parallel tests")]
public class SubcategoryIntegrationTest : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private readonly SmartwmsDbContext _dbContext;
    private readonly IServiceScope _scope;
    
    public SubcategoryIntegrationTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>(); 
    }

    private async Task<int> SeedDatabase()
    {
        var category = new Category
        {
            CategoryName = "Test category"
        };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        return category.CategoryId;
    }

    [Fact]
    public async Task AddSubcategory_ShouldReturnOk()
    {
        // Arrange
        var categoryId = await SeedDatabase();
        var subcategoryDto = new SubcategoryDto
        {
            SubcategoryName = "Test subcategory add",
            CategoriesCategoryId = categoryId
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/Subcategory", subcategoryDto);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        // Arrange
        var categoryId = await SeedDatabase();
        var subcategory = new Subcategory
        {
            SubcategoryName = "Test subcategory getId",
            CategoriesCategoryId = categoryId
        };

        await _dbContext.Subcategories.AddAsync(subcategory);
        await _dbContext.SaveChangesAsync();

        var subcategoryId = subcategory.SubcategoryId;
        
        // Act
        var response = await _client.GetAsync($"/api/Subcategory/{subcategoryId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<SubcategoryDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.SubcategoryId.Should().Be(subcategoryId);
        responseDto.SubcategoryName.Should().Be("Test subcategory getId");
        responseDto.CategoriesCategoryId.Should().Be(categoryId);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Arrange
        var categoryId = await SeedDatabase();
        var subcategory = new Subcategory
        {
            SubcategoryName = "Test subcategory getAll",
            CategoriesCategoryId = categoryId
        };
        
        await _dbContext.Subcategories.AddAsync(subcategory);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync("api/Subcategory");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDtos = JsonSerializer.Deserialize<List<SubcategoryDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDtos.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAllByCategory()
    {
        // Arrange
        var categoryId = await SeedDatabase();
        var subcategory = new Subcategory
        {
            SubcategoryName = "Test subcategory getAllByCategory",
            CategoriesCategoryId = categoryId
        };
        
        await _dbContext.Subcategories.AddAsync(subcategory);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync($"/api/Subcategory/byCategory/{categoryId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDtos = JsonSerializer.Deserialize<List<SubcategoryDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDtos.Should().NotBeEmpty();
        responseDtos!.All(x => x.CategoriesCategoryId == categoryId).Should().BeTrue();
    }

    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        // Arrange
        var categoryId = await SeedDatabase();
        var subcategory = new Subcategory
        {
            SubcategoryName = "Test subcategory update",
            CategoriesCategoryId = categoryId
        };
        
        var newSubcategory = new SubcategoryDto
        {
            SubcategoryName = "Test subcategory updated",
            CategoriesCategoryId = categoryId
        };
        
        await _dbContext.Subcategories.AddAsync(subcategory);
        await _dbContext.SaveChangesAsync();

        var subcategoryId = subcategory.SubcategoryId;
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/Subcategory/{subcategoryId}", newSubcategory);
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be("Subcategory edited");
    }

    [Fact]
    public async Task Delete_ShouldReturnOk()
    {
        // Arrange
        var categoryId = await SeedDatabase();
        var subcategory = new Subcategory
        {
            SubcategoryName = "Test subcategory delete",
            CategoriesCategoryId = categoryId
        };
        
        await _dbContext.Subcategories.AddAsync(subcategory);
        await _dbContext.SaveChangesAsync();
        
        var subcategoryId = subcategory.SubcategoryId;
        
        // Act
        var respose = await _client.DeleteAsync($"/api/Subcategory/{subcategoryId}");
        
        // Assert
        var content = await respose.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<Subcategory>(content, customJsonOptions);

        respose.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.SubcategoryId.Should().Be(subcategoryId);
        responseDto.SubcategoryName.Should().Be("Test subcategory delete");
        responseDto.IsDeleted.Should().BeTrue();
    }
}