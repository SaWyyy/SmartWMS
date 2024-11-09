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

public class LaneControllerTest
{
    private readonly ILaneRepository _laneRepository;
    private readonly LaneController _laneController;
    private readonly ILogger<LaneController> _logger;

    public LaneControllerTest()
    {
        this._laneRepository = A.Fake<ILaneRepository>();
        this._logger = A.Fake<ILogger<LaneController>>();
        this._laneController = new LaneController(_laneRepository, _logger);
    }

    private static Lane CreateFakeLane()
    {
        var lane = new Lane
        {
            LaneId = 1,
            LaneCode = "A01"
        };

        return lane;
    }

    private static LaneDto CreateFakeLaneDto()
    {
        var laneDto = new LaneDto
        {
            LaneId = 1,
            LaneCode = "A01"
        };

        return laneDto;
    }

    [Fact]
    public async void LaneController_AddLane_ReturnsOk()
    {
        // Arrange
        var lane = CreateFakeLane();
        var laneDto = CreateFakeLaneDto();
        
        // Act
        A.CallTo(() => _laneRepository.Add(laneDto)).Returns(lane);
        var result = (OkObjectResult)await _laneController.AddLane(laneDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void LaneController_AddLane_ReturnsBadRequest()
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        var laneDto = CreateFakeLaneDto();
        
        // Act
        A.CallTo(() => _laneRepository.Add(laneDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _laneController.AddLane(laneDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void LaneController_GetAll_ReturnsOk()
    {
        // Arrange
        var lanes = new List<LaneDto>();
        lanes.Add(CreateFakeLaneDto());
        
        // Act
        A.CallTo(() => _laneRepository.GetAll()).Returns(lanes);
        var result = (OkObjectResult)await _laneController.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void LaneControllerGetAllWithRacksShelves_ReturnsOk()
    {
        // Arrange
        var lanes = new List<LaneRacksShelvesDto>();
        lanes.Add(new LaneRacksShelvesDto());
        
        // Act
        A.CallTo(() => _laneRepository.GetAllWithRacksShelves()).Returns(lanes);
        var result = (OkObjectResult)await _laneController.GetAllWithRackShelves();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void LaneController_Get_ReturnsOk(int id)
    {
        // Arrange
        var laneDto = CreateFakeLaneDto();
        
        // Act
        A.CallTo(() => _laneRepository.Get(id)).Returns(laneDto);
        var result = (OkObjectResult)await _laneController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void LaneController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _laneRepository.Get(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _laneController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void LaneController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var lane = CreateFakeLane();
        
        // Act
        A.CallTo(() => _laneRepository.Delete(id)).Returns(lane);
        var result = (OkObjectResult)await _laneController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void LaneController_Delete_ReturnsBadRequest(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _laneRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _laneController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void LaneController_Update_ReturnsOk(int id)
    {
        // Arrange
        var lane = CreateFakeLane();
        var laneDto = CreateFakeLaneDto();
        
        // Act
        A.CallTo(() => _laneRepository.Update(id, laneDto)).Returns(lane);
        var result = (OkObjectResult)await _laneController.Update(id, laneDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void LaneController_Update_ReturnsBadRequest(int id)
    {
        // Arrange
        var laneDto = CreateFakeLaneDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _laneRepository.Update(id, laneDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _laneController.Update(id, laneDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
}