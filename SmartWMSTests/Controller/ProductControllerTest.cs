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
using SmartWMS.Services.Interfaces;

namespace SmartWMSTests.Controller;

public class ProductControllerTest
{
    private readonly IProductRepository _productRepository;
    private readonly IProductAssignmentService _service;
    private readonly ILogger<ProductController> _logger;
    private readonly ProductController _productController;

    public ProductControllerTest()
    {
        this._productRepository = A.Fake<IProductRepository>();
        this._logger = A.Fake<ILogger<ProductController>>();
        this._service = A.Fake<IProductAssignmentService>();
        this._productController = new ProductController(_productRepository, _service, _logger);
    }

    private static Product CreateFakeProduct()
    {
        var product = A.Fake<Product>();
        product.ProductId = 1;
        product.ProductDescription = "Test";
        product.Quantity = 1;
        product.Barcode = "Test";
        product.ProductName = "Test";
        product.Price = 1;
        product.Shelves = new List<Shelf>();
        product.OrderDetails = new List<OrderDetail>();
        product.SubcategoriesSubcategory = new Subcategory();
        product.WarehousesWarehouse = new Warehouse();
        product.SubcategoriesSubcategoryId = 1;
        product.WarehousesWarehouseId = 1;
        return product;
    }

    private static ProductDto CreateFakeProductDto()
    {
        var productDto = A.Fake<ProductDto>();
        productDto.ProductId = 1;
        productDto.ProductDescription = "Test";
        productDto.Quantity = 1;
        productDto.Barcode = "Test";
        productDto.ProductName = "Test";
        productDto.Price = 1;
        productDto.SubcategoriesSubcategoryId = 1;
        return productDto;
    }

    [Fact]
    public async void ProductController_Add_ReturnsOk()
    {
        // Arrange
        var product = CreateFakeProduct();
        var productDto = CreateFakeProductDto();
        
        // Act
        A.CallTo(() => _productRepository.Add(productDto)).Returns(product);
        var result = (OkObjectResult)await _productController.Add(productDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void ProductController_Add_ReturnsBadRequest()
    {
        // Arrange
        var productDto = CreateFakeProductDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _productRepository.Add(productDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _productController.Add(productDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void ProductController_GetAll_ReturnsOk()
    {
        // Arrange
        var products = new List<ProductDto>();
        products.Add(CreateFakeProductDto());
        
        // Act
        A.CallTo(() => _productRepository.GetAll()).Returns(products);
        var result = (OkObjectResult)await _productController.GetAll();
        
        // Arrange
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ProductController_Get_ReturnsOk(int id)
    {
        // Arrange
        var productDto = CreateFakeProductDto();
        
        // Act
        A.CallTo(() => _productRepository.Get(id)).Returns(productDto);
        var result = (OkObjectResult)await _productController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ProductController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _productRepository.Get(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _productController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ProductController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var product = CreateFakeProduct();
        product.ProductId = id;
        
        // Act
        A.CallTo(() => _productRepository.Delete(id)).Returns(product);
        var result = (OkObjectResult)await _productController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ProductController_Delete_ReturnsBadRequest(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _productRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _productController.Delete(id);
        
        //Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ProductController_Delete_ReturnsConflict(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _productRepository.Delete(id))
            .Throws(new ConflictException(exceptionMessage));
        var result = (ConflictObjectResult)await _productController.Delete(id);
        
        //Assert
        result.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        result.Should().NotBeNull();
    }
    
    

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ProductController_Update_ReturnsOk(int id)
    {
        // Arrange
        var product = CreateFakeProduct();
        var productDto = CreateFakeProductDto();
        product.ProductId = id;
        
        // Act
        A.CallTo(() => _productRepository.Update(id, productDto)).Returns(product);
        var result = (OkObjectResult)await _productController.Update(id, productDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ProductController_Update_ReturnsBadRequest(int id)
    {
        // Arrange
        var productDto = CreateFakeProductDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _productRepository.Update(id, productDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _productController.Update(id, productDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
}