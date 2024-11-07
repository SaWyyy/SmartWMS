using System.Runtime.InteropServices;
using Castle.Core.Logging;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartWMS;
using SmartWMS.Controllers;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMSTests.Controller;

public class SubcategoryControllerTest
{
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly ILogger<SubcategoryController> _logger;
    private readonly SubcategoryController _subcategoryController;

    public SubcategoryControllerTest()
    {
        this._subcategoryRepository = A.Fake<ISubcategoryRepository>();
        this._logger = A.Fake<ILogger<SubcategoryController>>();
        this._subcategoryController = new SubcategoryController(_logger, _subcategoryRepository);
    }

    private static Subcategory CreateFakeSubcategory()
    {
        var subcategory = A.Fake<Subcategory>();
        subcategory.SubcategoryId = 1;
        subcategory.SubcategoryName = "Test";
        subcategory.CategoriesCategoryId = 1;
        subcategory.CategoriesCategory = new Category();
        subcategory.Products = new List<Product>();
        return subcategory;
    }

    private static SubcategoryDto CreateFakeSubcategoryDto()
    {
        var subcategoryDto = A.Fake<SubcategoryDto>();
        subcategoryDto.SubcategoryId = 1;
        subcategoryDto.SubcategoryName = "Test";
        subcategoryDto.CategoriesCategoryId = 1;
        return subcategoryDto;
    }

    [Fact]
    public async void SubcategoryController_Add_ReturnsOk()
    {
        // Arrange
        var subcategory = CreateFakeSubcategory();
        var subcategoryDto = CreateFakeSubcategoryDto();
        
        // Act
        A.CallTo(() => _subcategoryRepository.Add(subcategoryDto)).Returns(subcategory);
        var result = (OkObjectResult)await _subcategoryController.Add(subcategoryDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void SubcategoryController_Add_ReturnsBadRequest()
    {
        // Arrange
        var subcategoryDto = CreateFakeSubcategoryDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _subcategoryRepository.Add(subcategoryDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _subcategoryController.Add(subcategoryDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void SubcategoryController_GetAll_ReturnsOk()
    {
        // Arrange
        var subcategories = new List<SubcategoryDto>();
        subcategories.Add(CreateFakeSubcategoryDto());
        
        // Act
        A.CallTo(() => _subcategoryRepository.GetAll()).Returns(subcategories);
        var result = (OkObjectResult)await _subcategoryController.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void SubcategoryController_GetAllByCategory(int categoryId)
    {
        // Arrange
        var subcategories = new List<SubcategoryDto>();
        subcategories.Add(CreateFakeSubcategoryDto());
        
        // Act
        A.CallTo(() => _subcategoryRepository.GetAllByCategory(categoryId)).Returns(subcategories);
        var result = (OkObjectResult)await _subcategoryController.GetAllByCategory(categoryId);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void SubcategoryController_Get_ReturnsOk(int id)
    {
        // Arrange
        var subcategoryDto = CreateFakeSubcategoryDto();
        subcategoryDto.SubcategoryId = id;
        
        // Act
        A.CallTo(() => _subcategoryRepository.Get(id)).Returns(subcategoryDto);
        var result = (OkObjectResult)await _subcategoryController.Get(id);
        
        // Assert
        result.StatusCode.Should().NotBeNull();
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void SubcategoryController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _subcategoryRepository.Get(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _subcategoryController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void SubcategoryController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var subcategory = CreateFakeSubcategory();
        subcategory.SubcategoryId = id;
        
        // Act
        A.CallTo(() => _subcategoryRepository.Delete(id)).Returns(subcategory);
        var result = (OkObjectResult)await _subcategoryController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void SubcategoryController_Delete_ReturnsBadRequest(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _subcategoryRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _subcategoryController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void SubcategoryController_Delete_ReturnsConflict(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _subcategoryRepository.Delete(id))
            .Throws(new ConflictException(exceptionMessage));
        var result = (ConflictObjectResult)await _subcategoryController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void SubcategoryController_Update_ReturnsOk(int id)
    {
        // Arrange
        var subcategory = CreateFakeSubcategory();
        var subcategoryDto = CreateFakeSubcategoryDto();
        subcategory.SubcategoryId = id;
        
        // Act
        A.CallTo(() => _subcategoryRepository.Update(id, subcategoryDto)).Returns(subcategory);
        var result = (OkObjectResult)await _subcategoryController.Update(id, subcategoryDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void SubcategoryController_Update_ReturnsBadRequest(int id)
    {
        // Arrange
        var subcategoryDto = CreateFakeSubcategoryDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _subcategoryRepository.Update(id, subcategoryDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _subcategoryController.Update(id, subcategoryDto);
    }
}