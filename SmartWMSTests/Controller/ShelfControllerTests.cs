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
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMSTests.Controller;

public class ShelfControllerTests
{
    private readonly IShelfRepository _shelfRepository;
    private readonly ILogger<ShelfController> _logger;
    private readonly ShelfController _shelfController;

    public ShelfControllerTests()
    {
        this._shelfRepository = A.Fake<IShelfRepository>();
        this._logger = A.Fake<ILogger<ShelfController>>();
        this._shelfController = new ShelfController(_shelfRepository, _logger);
    }

    public static Shelf CreateFakeShelf()
    {
        var shelf = A.Fake<Shelf>();
        shelf.ShelfId = 1;
        shelf.Level = LevelType.P0;
        shelf.CurrentQuant = 1;
        shelf.MaxQuant = 1;
        shelf.ProductsProduct = new Product();
        shelf.ProductsProductId = 1;
        return shelf;
    }

    public static ShelfDto CreateFakeShelfDto()
    {
        var shelfDto = A.Fake<ShelfDto>();
        shelfDto.ShelfId = 1;
        shelfDto.Level = LevelType.P0;
        shelfDto.CurrentQuant = 1;
        shelfDto.MaxQuant = 1;
        shelfDto.ProductsProductId = 1;
        return shelfDto;
    }

    [Fact]
    public async void ShelfController_Add_ReturnsOk()
    {
        // Arrange
        var shelf = CreateFakeShelf();
        var shelfDto = CreateFakeShelfDto();
        
        // Act
        A.CallTo(() => _shelfRepository.AddShelf(shelfDto)).Returns(shelf);
        var result = (OkObjectResult)await _shelfController.AddShelf(shelfDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void ShelfController_Add_ReturnsBadRequest()
    {
        // Arrange
        var shelfDto = CreateFakeShelfDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _shelfRepository.AddShelf(shelfDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _shelfController.AddShelf(shelfDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void ShelfController_GetAll_ReturnsOk()
    {
        // Arrange
        var shelves = new List<ShelfDto>();
        shelves.Add(CreateFakeShelfDto());
        
        // Act
        A.CallTo(() => _shelfRepository.GetAll()).Returns(shelves);
        var result = (OkObjectResult)await _shelfController.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ShelfController_Get_ReturnsOk(int id)
    {
        // Arrange
        var shelfDto = CreateFakeShelfDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _shelfRepository.Get(id)).Returns(shelfDto);
        var result = (OkObjectResult)await _shelfController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ShelfController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _shelfRepository.Get(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _shelfController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ShelfController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var shelf = CreateFakeShelf();
        shelf.ShelfId = id;

        // Act
        A.CallTo(() => _shelfRepository.Delete(id)).Returns(shelf);
        var result = (OkObjectResult)await _shelfController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ShelfController_Delete_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _shelfRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _shelfController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ShelfController_Update_ReturnsOk(int id)
    {
        // Arrange
        var shelf = CreateFakeShelf();
        var shelfDto = CreateFakeShelfDto();
        shelf.ShelfId = id;
        
        // Act
        A.CallTo(() => _shelfRepository.Update(id, shelfDto)).Returns(shelf);
        var result = (OkObjectResult)await _shelfController.Update(id, shelfDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ShelfController_Update_ReturnsBadRequest(int id)
    {
        // Arrange
        var shelfDto = CreateFakeShelfDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _shelfRepository.Update(id, shelfDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _shelfController.Update(id, shelfDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
}