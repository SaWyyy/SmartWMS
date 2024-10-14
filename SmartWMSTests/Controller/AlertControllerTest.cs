using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartWMS.Controllers;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMSTests.Controller;

public class AlertControllerTest
{
    private readonly IAlertRepository _alertRepository;
    private readonly AlertController _alertController;
    private readonly ILogger<AlertController> _logger;

    public AlertControllerTest()
    {
        this._alertRepository = A.Fake<IAlertRepository>();
        this._logger = A.Fake<ILogger<AlertController>>();
        this._alertController = new AlertController(_alertRepository, _logger);
    }

    private static AlertDto CreateFakeAlertDto()
    {
        var alertDto = A.Fake<AlertDto>();
        alertDto.AlertId = 1;
        alertDto.Seen = false;
        alertDto.Title = "Test alert";
        alertDto.Description = "Test description";
        alertDto.AlertDate = new DateTime();
        alertDto.AlertType = AlertType.DeliveryCanceled;
        return alertDto;
    }

    private static Alert CreateFakeAlert()
    {
        var alert = A.Fake<Alert>();
        alert.AlertId = 1;
        alert.Seen = false;
        alert.Title = "Test alert";
        alert.Description = "Test description";
        alert.AlertDate = new DateTime();
        alert.AlertType = AlertType.DeliveryCanceled;
        return alert;
    }
    

    [Fact]
    public async void AlertController_Add_ReturnOk()
    {
        // Arrange
        var alertDto = CreateFakeAlertDto();
        var alert = CreateFakeAlert();
        
        // Act
        A.CallTo(() => _alertRepository.Add(alertDto)).Returns(alert);
        var result = (OkObjectResult)await _alertController.AddAlert(alertDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async void AlertController_GetAll_ReturnsOk()
    {
        // Arrange
        var alerts = A.Fake<List<AlertDto>>();
        alerts.Add(CreateFakeAlertDto());
        
        // Act
        A.CallTo(() => _alertRepository.GetAll()).Returns(alerts);
        var result = (OkObjectResult)await _alertController.GetAll();
        
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void AlertController_Get_ReturnsOk(int id)
    {
        // Arrange
        var alertDto = CreateFakeAlertDto();
        alertDto.AlertId = id;
        
        // Act
        A.CallTo(() => _alertRepository.Get(id)).Returns(alertDto);
        var result = (OkObjectResult)await _alertController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void AlertController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var alert = CreateFakeAlert();
        alert.AlertId = id;
        
        // Act
        A.CallTo(() => _alertRepository.Delete(id)).Returns(alert);
        var result = (OkObjectResult) await _alertController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void AlertController_Update_ReturnsOk(int id)
    {
        // Arrange
        var alertDto = CreateFakeAlertDto();
        var alert = CreateFakeAlert();
        alert.AlertId = id;
        
        // Act
        A.CallTo(() => _alertRepository.Update(id, alertDto)).Returns(alert);
        var result = (OkObjectResult)await _alertController.Update(id, alertDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void AlertController_ChangeSeen_ReturnsOk(int id)
    {
        // Arrange
        var alert = CreateFakeAlert();
        alert.AlertId = id;
        
        // Act
        A.CallTo(() => _alertRepository.ChangeSeen(id)).Returns(alert);
        var result = (OkObjectResult)await _alertController.ChangeSeen(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }
}