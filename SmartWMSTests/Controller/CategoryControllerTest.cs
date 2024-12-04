using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartWMS;
using SmartWMS.Controllers;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.ResponseDTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMSTests.Controller;

public class CategoryControllerTest
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly CategoryController _categoryController;
    private readonly ILogger<CategoryController> _logger;

    public CategoryControllerTest()
    {
        this._categoryRepository = A.Fake<ICategoryRepository>();
        this._logger = A.Fake<ILogger<CategoryController>>();
        this._categoryController = new CategoryController(_categoryRepository, _logger);
    }

    private static CategoryDto CreateFakeCategoryDto()
    {
        var categoryDto = A.Fake<CategoryDto>();
        categoryDto.CategoryId = 1;
        categoryDto.CategoryName = "Test name";
        return categoryDto;
    }

    private static Category CreateFakeCategory()
    {
        var category = A.Fake<Category>();
        category.CategoryId = 1;
        category.CategoryName = "Test name";
        category.Subcategories = new List<Subcategory>();
        return category;
    }

    private static CategorySubcategoriesDto CreateFakeCategorySubcategoriesDto()
    {
        var categoryDto = A.Fake<CategorySubcategoriesDto>();
        categoryDto.CategoryId = 1;
        categoryDto.CategoryName = "Test name";
        categoryDto.Subcategories = new List<SubcategoryDto>();
        return categoryDto;
    }

    [Fact]
    public async void CategoryController_Add_ReturnsOk()
    {
        // Arrange
        var category = CreateFakeCategory();
        var categoryDto = CreateFakeCategoryDto();
        
        // Act
        A.CallTo(() => _categoryRepository.AddCategory(categoryDto)).Returns(category);
        var result = (OkObjectResult)await _categoryController.AddCategory(categoryDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void CategoryController_Add_ReturnsBadRequest()
    {
        // Arrange
        var categoryDto = CreateFakeCategoryDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _categoryRepository.AddCategory(categoryDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (ObjectResult)await _categoryController.AddCategory(categoryDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void CategoryController_GetAll_ReturnsOk()
    {
        // Arrange
        var categories = A.Fake<List<CategoryDto>>();
        categories.Add(CreateFakeCategoryDto());

        // Act
        A.CallTo(() => _categoryRepository.GetAll()).Returns(categories);
        var result = (OkObjectResult)await _categoryController.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async void CategoryController_GetWithSubcategories_ReturnsOk()
    {
        // Arrange
        var categories = A.Fake<List<CategorySubcategoriesDto>>();
        categories.Add(CreateFakeCategorySubcategoriesDto());
        
        // Act
        A.CallTo(() => _categoryRepository.GetWithSubcategories()).Returns(categories);
        var result = (OkObjectResult)await _categoryController.GetAllWithSubcategories();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CategoryController_Get_ReturnsOk(int id)
    {
        // Arrange
        var categoryDto = CreateFakeCategoryDto();
        categoryDto.CategoryId = id;
        
        // Act
        A.CallTo(() => _categoryRepository.GetCategory(id)).Returns(categoryDto);
        var result = (OkObjectResult)await _categoryController.GetCategory(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CategoryController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _categoryRepository.GetCategory(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _categoryController.GetCategory(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CategoryController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var category = CreateFakeCategory();
        category.CategoryId = id;
        
        // Act
        A.CallTo(() => _categoryRepository.Delete(id)).Returns(category);
        var result = (OkObjectResult)await _categoryController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CategoryController_Delete_ReturnsBadRequest(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _categoryRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _categoryController.Delete(id);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CategoryController_Delete_ReturnsConflict(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _categoryRepository.Delete(id))
            .Throws(new ConflictException(exceptionMessage));
        var result = (ConflictObjectResult)await _categoryController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CategoryController_Update_ReturnsOk(int id)
    {
        // Arrange
        var categoryDto = CreateFakeCategoryDto();
        var category = CreateFakeCategory();
        category.CategoryId = id;
        
        // Act
        A.CallTo(() => _categoryRepository.Update(id, categoryDto)).Returns(category);
        var result = (OkObjectResult)await _categoryController.Update(id, categoryDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CategoryController_Update_ReturnsBadRequest(int id)
    {
        // Arrange
        var categoryDto = CreateFakeCategoryDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _categoryRepository.Update(id, categoryDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _categoryController.Update(id, categoryDto);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
}