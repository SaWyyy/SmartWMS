using System.Runtime.InteropServices;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using SmartWMS;
using SmartWMS.Controllers;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;
using ILogger = NLog.ILogger;

namespace SmartWMSTests.Controller;

public class OrderHeaderControllerTest
{
    private readonly IOrderHeaderRepository _orderHeaderRepository;
    private readonly ILogger<OrderHeaderController> _logger;
    private readonly OrderHeaderController _orderHeaderController;

    public OrderHeaderControllerTest()
    {
        this._orderHeaderRepository = A.Fake<IOrderHeaderRepository>();
        this._logger = A.Fake<ILogger<OrderHeaderController>>();
        this._orderHeaderController = new OrderHeaderController(_orderHeaderRepository, _logger);
    }

    private static OrderHeader CreateFakeOrderHeader()
    {
        var orderHeader = A.Fake<OrderHeader>();
        orderHeader.OrdersHeaderId = 1;
        orderHeader.OrderDate = new DateTime();
        orderHeader.DeliveryDate = new DateTime();
        orderHeader.DestinationAddress = "Test address";
        orderHeader.StatusName  = OrderName.Cancelled;
        orderHeader.TypeName = OrderType.Delivery;
        orderHeader.OrderDetails = new List<OrderDetail>();
        orderHeader.WaybillsWaybill = new Waybill();
        return orderHeader;
    }

    private static OrderHeaderDto CreateFakeOrderHeaderDto()
    {
        var orderHeaderDto = A.Fake<OrderHeaderDto>();
        orderHeaderDto.OrdersHeaderId = 1;
        orderHeaderDto.OrderDate = new DateTime();
        orderHeaderDto.DeliveryDate = new DateTime();
        orderHeaderDto.DestinationAddress = "Test address";
        orderHeaderDto.StatusName = OrderName.Cancelled;
        orderHeaderDto.TypeName = OrderType.Delivery;
        return orderHeaderDto;
    }

    [Fact]
    public async void OrderHeaderController_Add_ReturnsOk()
    {
        // Arrange
        var orderHeader = CreateFakeOrderHeader();
        var orderHeaderDto = CreateFakeOrderHeaderDto();
        
        // Act
        A.CallTo(() => _orderHeaderRepository.Add(orderHeaderDto)).Returns(orderHeader);
        var result = (OkObjectResult)await _orderHeaderController.AddOrderHeader(orderHeaderDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void OrderHeaderController_Add_ReturnsBadRequest()
    {
        // Arrange
        var orderHeaderDto = CreateFakeOrderHeaderDto();
        const string exceptionMessage = "An error occured";

        // Act
        A.CallTo(() => _orderHeaderRepository.Add(orderHeaderDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _orderHeaderController.AddOrderHeader(orderHeaderDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void OrderHeaderController_GetAll_ReturnsOk()
    {
        // Arrange
        var orderHeaders = new List<OrderHeaderDto>();
        orderHeaders.Add(CreateFakeOrderHeaderDto());
        
        // Act
        A.CallTo(() => _orderHeaderRepository.GetAll()).Returns(orderHeaders);
        var result = (OkObjectResult)await _orderHeaderController.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderHeaderController_Get_ReturnsOk(int id)
    {
        // Arrange
        var orderHeaderDto = CreateFakeOrderHeaderDto();
        orderHeaderDto.OrdersHeaderId = id;
        
        // Act
        A.CallTo(() => _orderHeaderRepository.Get(id)).Returns(orderHeaderDto);
        var result = (OkObjectResult)await _orderHeaderController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderHeaderController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exeptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _orderHeaderRepository.Get(id))
            .Throws(new SmartWMSExceptionHandler(exeptionMessage));
        var result = (NotFoundObjectResult)await _orderHeaderController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderHeaderController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var orderHeader = CreateFakeOrderHeader();
        orderHeader.OrdersHeaderId = id;
        
        // Act
        A.CallTo(() => _orderHeaderRepository.Delete(id)).Returns(orderHeader);
        var result = (OkObjectResult)await _orderHeaderController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderHeaderController_Delete_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _orderHeaderRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _orderHeaderController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderHeaderController_Update_ReturnsOk(int id)
    {
        // Arrange
        var orderHeader = CreateFakeOrderHeader();
        var orderHeaderDto = CreateFakeOrderHeaderDto();
        orderHeader.OrdersHeaderId = id;
        
        // Act
        A.CallTo(() => _orderHeaderRepository.Update(id, orderHeaderDto)).Returns(orderHeader);
        var result = (OkObjectResult)await _orderHeaderController.Update(id, orderHeaderDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void OrderHeaderController_Update_ReturnsBadReques(int id)
    {
        // Arrange
        var orderHeaderDto = CreateFakeOrderHeaderDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _orderHeaderRepository.Update(id, orderHeaderDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _orderHeaderController.Update(id, orderHeaderDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
}