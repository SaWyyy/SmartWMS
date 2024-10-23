using System.Runtime.InteropServices;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartWMS;
using SmartWMS.Controllers;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMSTests.Controller;

public class WaybillControllerTest
{
    private readonly IWaybillRepository _waybillRepository;
    private readonly ILogger<WaybillController> _logger;
    private readonly WaybillController _waybillController;

    public WaybillControllerTest()
    {
        this._waybillRepository = A.Fake<IWaybillRepository>();
        this._logger = A.Fake<ILogger<WaybillController>>();
        this._waybillController = new WaybillController(_waybillRepository, _logger);
    }

    private static Waybill CreateFakeWaybill()
    {
        var waybill = A.Fake<Waybill>();
        waybill.WaybillId = 1;
        waybill.LoadingDate = new DateTime();
        waybill.PostalCode = "Test";
        waybill.ShippingDate = new DateTime();
        waybill.OrderHeadersOrderHeaderId = 1;
        waybill.CountriesCountryId = 1;
        waybill.SupplierName = "Test";
        waybill.CountriesCountry = new Country();
        waybill.OrderHeadersOrderHeader = new OrderHeader();
        return waybill;
    }

    private static WaybillDto CreateFakeWaybillDto()
    {
        var waybillDto = A.Fake<WaybillDto>();
        waybillDto.WaybillId = 1;
        waybillDto.LoadingDate = new DateTime();
        waybillDto.ShippingDate = new DateTime();
        waybillDto.PostalCode = "Test";
        waybillDto.SupplierName = "Test";
        waybillDto.CountriesCountryId = 1;
        waybillDto.OrderHeadersOrderHeaderId = 1;
        return waybillDto;
    }

    [Fact]
    public async void WaybillController_Add_ReturnsOk()
    {
        // Arrange
        var waybill = CreateFakeWaybill();
        var waybillDto = CreateFakeWaybillDto();
        
        // Act
        A.CallTo(() => _waybillRepository.AddWaybill(waybillDto)).Returns(waybill);
        var result = (OkObjectResult)await _waybillController.AddWaybill(waybillDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void WaybillController_Add_ReturnsBadRequest()
    {
        // Arrange
        var waybillDto = CreateFakeWaybillDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _waybillRepository.AddWaybill(waybillDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _waybillController.AddWaybill(waybillDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void WaybillController_GetAll_ReturnsOk()
    {
        // Arrange
        var waybills = new List<WaybillDto>();
        waybills.Add(CreateFakeWaybillDto());
        
        // Act
        A.CallTo(() => _waybillRepository.GetAll()).Returns(waybills);
        var result = (OkObjectResult)await _waybillController.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void WaybillController_Get_ReturnsOk(int id)
    {
        // Arrange
        var waybillDto = CreateFakeWaybillDto();
        
        // Act
        A.CallTo(() => _waybillRepository.Get(id)).Returns(waybillDto);
        var result = (OkObjectResult)await _waybillController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void WaybillController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _waybillRepository.Get(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _waybillController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void WaybillController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var waybill = CreateFakeWaybill();
        waybill.WaybillId = id;
        
        // Act
        A.CallTo(() => _waybillRepository.Delete(id)).Returns(waybill);
        var result = (OkObjectResult)await _waybillController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void WaybillController_Delete_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _waybillRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _waybillController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void WaybillController_Update_ReturnsOk(int id)
    {
        // Arrange
        var waybill = CreateFakeWaybill();
        var waybillDto = CreateFakeWaybillDto();
        waybill.WaybillId = id;
        
        // Act
        A.CallTo(() => _waybillRepository.Update(id, waybillDto)).Returns(waybill);
        var result = (OkObjectResult)await _waybillController.Update(id, waybillDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void WaybillController_Update_ReturnsBadRequest(int id)
    {
        // Arrange
        var waybillDto = CreateFakeWaybillDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _waybillRepository.Update(id, waybillDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _waybillController.Update(id, waybillDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
}