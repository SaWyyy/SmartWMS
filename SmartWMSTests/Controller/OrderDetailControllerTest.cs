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
using Task = SmartWMS.Entities.Task;

namespace SmartWMSTests.Controller;

public class OrderDetailControllerTest
{
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly ILogger<OrderDetailController> _logger;
    private readonly OrderDetailController _orderDetailController;

    public OrderDetailControllerTest()
    {
        this._orderDetailRepository = A.Fake<IOrderDetailRepository>();
        this._logger = A.Fake<ILogger<OrderDetailController>>();
        this._orderDetailController = new OrderDetailController(_orderDetailRepository, _logger);
    }

    private static OrderDetail CreateFakeOrderDetail()
    {
        var orderDetail = A.Fake<OrderDetail>();
        orderDetail.OrderDetailId = 1;
        orderDetail.Quantity = 1;
        orderDetail.OrderHeadersOrdersHeaderId = 1;
        orderDetail.ProductsProductId = 1;
        orderDetail.OrderHeadersOrdersHeader = new OrderHeader();
        orderDetail.ProductsProduct = new Product();
        orderDetail.TasksTask = new List<Task>();
        return orderDetail;
    }

    private static OrderDetailDto CreateFakeOrderDetailDto()
    {
        var orderDetailDto = A.Fake<OrderDetailDto>();
        orderDetailDto.OrderDetailId = 1;
        orderDetailDto.OrderHeadersOrdersHeaderId = 1;
        orderDetailDto.Quantity = 1;
        orderDetailDto.ProductsProductId = 1;
        return orderDetailDto;
    }

    [Fact]
    public async void OrderDetailController_Add_ReturnsOk()
    {
        // Arrange
        var orderDetail = CreateFakeOrderDetail();
        var orderDetailDto = CreateFakeOrderDetailDto();
        
        // Act
        A.CallTo(() => _orderDetailRepository.Add(orderDetailDto)).Returns(orderDetail);
        var result = (OkObjectResult)await _orderDetailController.AddOrderDetail(orderDetailDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void OrderDetailController_Add_ReturnsBadRequest()
    {
        // Arrange
        var orderDetailDto = CreateFakeOrderDetailDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _orderDetailRepository.Add(orderDetailDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _orderDetailController.AddOrderDetail(orderDetailDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void OrderDetailController_GetAll_ReturnsOk()
    {
        // Arrange
        var orderDetails = new List<OrderDetailDto>();
        orderDetails.Add(CreateFakeOrderDetailDto());
        
        // Act
        A.CallTo(() => _orderDetailRepository.GetAll()).Returns(orderDetails);
        var result = (OkObjectResult)await _orderDetailController.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderDetailController_Get_ReturnsOk(int id)
    {
        // Arrange
        var orderDetailDto = CreateFakeOrderDetailDto();
        orderDetailDto.OrderDetailId = id;
        
        // Act
        A.CallTo(() => _orderDetailRepository.Get(id)).Returns(orderDetailDto);
        var result = (OkObjectResult)await _orderDetailController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderDetailController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _orderDetailRepository.Get(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _orderDetailController.Get(id);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderDetailController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var orderDetail = CreateFakeOrderDetail();
        orderDetail.OrderDetailId = id;
        
        // Act
        A.CallTo(() => _orderDetailRepository.Delete(id)).Returns(orderDetail);
        var result = (OkObjectResult)await _orderDetailController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderDetailController_Delete_ReturnsBadRequest(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _orderDetailRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _orderDetailController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderDetailController_Update_ReturnsOk(int id)
    {
        // Arrange
        var orderDetail = CreateFakeOrderDetail();
        var orderDetailDto = CreateFakeOrderDetailDto();
        orderDetail.OrderDetailId = id;
        
        // Act
        A.CallTo(() => _orderDetailRepository.Update(id, orderDetailDto)).Returns(orderDetail);
        var result = (OkObjectResult)await _orderDetailController.Update(id, orderDetailDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderDetailController_Update_ReturnsBadRequest(int id)
    {
        // Arrange
        var orderDetailDto = CreateFakeOrderDetailDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _orderDetailRepository.Update(id, orderDetailDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _orderDetailController.Update(id, orderDetailDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
}