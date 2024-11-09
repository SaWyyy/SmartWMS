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

public class RackControllerTest
{
    private readonly IRackRepository _rackRepository;
    private readonly RackController _rackController;
    private readonly ILogger<RackController> _logger;

    public RackControllerTest()
    {
        this._rackRepository = A.Fake<IRackRepository>();
        this._logger = A.Fake<ILogger<RackController>>();
        this._rackController = new RackController(_rackRepository, _logger);
    }

    private static Rack CreateFakeRack()
    {
        var rack = A.Fake<Rack>();
        rack.RackId = 1;
        rack.RackNumber = 1;
        rack.Shelves = new List<Shelf>();
        return rack;
    }

    private static RackDto CreateFakeRackDto()
    {
        var rackDto = A.Fake<RackDto>();
        rackDto.RackId = 1;
        rackDto.RackNumber = 1;
        return rackDto;
    }

    [Fact]
    public async void RackController_Add_ReturnsOk()
    {
        // Arrange
        var rack = CreateFakeRack();
        var rackDto = CreateFakeRackDto();
        
        // Act
        A.CallTo(() => _rackRepository.Add(rackDto)).Returns(rack);
        var result = (OkObjectResult)await _rackController.AddRack(rackDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void RackController_Add_ReturnsBadRequest()
    {
        // Arrange
        var laneDto = CreateFakeRackDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _rackRepository.Add(laneDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _rackController.AddRack(laneDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void RackController_GetAll_ReturnsOk()
    {
        // Arrange
        var racks = new List<RackDto>();
        racks.Add(CreateFakeRackDto());
        
        // Act
        A.CallTo(() => _rackRepository.GetAll()).Returns(racks);
        var result = (OkObjectResult)await _rackController.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void RackController_Get_ReturnsOk(int id)
    {
        // Arrange
        var rackDto = CreateFakeRackDto();
        
        // Act
        A.CallTo(() => _rackRepository.Get(id)).Returns(rackDto);
        var result = (OkObjectResult)await _rackController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void RackController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _rackRepository.Get(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _rackController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void RackController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var rack = CreateFakeRack();
        
        // Act
        A.CallTo(() => _rackRepository.Delete(id)).Returns(rack);
        var result = (OkObjectResult)await _rackController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void RackController_Delete_ReturnsBadRequest(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _rackRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _rackController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void RackController_Update_ReturnsOk(int id)
    {
        // Arrange
        var rack = CreateFakeRack();
        var rackDto = CreateFakeRackDto();
        
        // Act
        A.CallTo(() => _rackRepository.Update(id, rackDto)).Returns(rack);
        var result = (OkObjectResult)await _rackController.Update(id, rackDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void RackController_Update_ReturnsBadRequest(int id)
    {
        // Arrange
        var rackDto = CreateFakeRackDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _rackRepository.Update(id, rackDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _rackController.Update(id, rackDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
}