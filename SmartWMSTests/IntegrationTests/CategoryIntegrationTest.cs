using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMSTests.IntegrationTests.Configuration;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Task = System.Threading.Tasks.Task;

namespace SmartWMSTests.IntegrationTests;

[Collection("Non-Parallel tests")]
public class CategoryIntegrationTest : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private readonly SmartwmsDbContext _dbContext;
    private readonly IServiceScope _scope;
    
    public CategoryIntegrationTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
    }

    [Fact]
    public async Task AddCategory_ShouldReturnOk()
    {
        // Arrange
        var categoryDto = new CategoryDto
        {
            CategoryName = "Test category add"
        };
        
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/Category", categoryDto);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        // Arrange
        var category = new Category
        {
            CategoryName = "Test category getId"
        };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var categoryId = category.CategoryId;
        
        // Act
        var response = await _client.GetAsync($"/api/Category/{categoryId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<CategoryDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.CategoryId.Should().Be(categoryId);
        responseDto.CategoryName.Should().Be("Test category getId");
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Arrange
        var category = new Category
        {
            CategoryName = "Test category getAll"
        };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync("/api/Category");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDtos = JsonSerializer.Deserialize<List<CategoryDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDtos.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetWithSubcategories_ShouldReturnOk()
    {
        // Arrange
        var category = new Category
        {
            CategoryName = "Test category withSubcategories"
        };
        
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        
        var subcategory = new Subcategory
        {
            SubcategoryName = "Test subcategory",
            CategoriesCategoryId = category.CategoryId
        };
        
        await _dbContext.Subcategories.AddAsync(subcategory);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync("/api/Category/withSubcategories");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<List<CategorySubcategoriesDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        // Arrange
        var category = new Category
        {
            CategoryName = "Test category update"
        };

        var newCategory = new CategoryDto
        {
            CategoryName = "Test category updated"
        };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var categoryId = category.CategoryId;

        // Act
        var response = await _client.PutAsJsonAsync($"/api/Category/{categoryId}", newCategory);
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<CategoryDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.CategoryId.Should().Be(categoryId);
        responseDto.CategoryName.Should().Be("Test category updated");
    }

    [Fact]
    public async Task Delete_ShouldReturnOk()
    {
        // Arrange
        var category = new Category
        {
            CategoryName = "Test category delete"
        };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var categoryId = category.CategoryId;
        
        // Act
        var response = await _client.DeleteAsync($"/api/Category/{categoryId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<Category>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.CategoryId.Should().Be(categoryId);
        responseDto.CategoryName.Should().Be("Test category delete");
        responseDto.IsDeleted.Should().BeTrue();
    }
}