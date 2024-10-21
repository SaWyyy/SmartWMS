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

public class ReportControllerTests
{
    private readonly IReportRepository _reportRepository;
    private readonly ILogger<ReportController> _logger;
    private readonly ReportController _reportController;

    public ReportControllerTests()
    {
        this._reportRepository = A.Fake<IReportRepository>();
        this._logger = A.Fake<ILogger<ReportController>>();
        this._reportController = new ReportController(_reportRepository, _logger);
    }

    private static Report CreateFakeReport()
    {
        var report = A.Fake<Report>();
        report.ReportId = 1;
        report.ReportFile = new byte[8];
        report.ReportDate = new DateTime();
        report.ReportPeriod = ReportPeriod.Day;
        report.ReportType = ReportType.Deliveries;
        report.WarehousesWarehouse = new Warehouse();
        report.WarehousesWarehouseId = 1;
        return report;
    }

    private static ReportDto CreateFakeReportDto()
    {
        var reportDto = A.Fake<ReportDto>();
        reportDto.ReportId = 1;
        reportDto.ReportDate = new DateTime();
        reportDto.ReportPeriod = ReportPeriod.Day;
        reportDto.ReportType = ReportType.Deliveries;
        return reportDto;
    }

    [Fact]
    public async void ReportController_Add_ReturnsOk()
    {
        // Arrange
        var report = CreateFakeReport();
        var reportDto = CreateFakeReportDto();

        // Act
        A.CallTo(() => _reportRepository.Add(reportDto)).Returns(report);
        var result = (OkObjectResult)await _reportController.Add(reportDto);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void ReportController_Add_ReturnsBadRequest()
    {
        // Arrange
        var reportDto = CreateFakeReportDto();
        const string exceptionMessage = "An error occured";

        // Act
        A.CallTo(() => _reportRepository.Add(reportDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _reportController.Add(reportDto);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void ReportController_GetAll_ReturnOk()
    {
        // Arrange
        var reports = new List<ReportDto>();
        reports.Add(CreateFakeReportDto());

        // Act
        A.CallTo(() => _reportRepository.GetAll()).Returns(reports);
        var result = (OkObjectResult)await _reportController.GetAll();

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ReportController_Get_ReturnsOk(int id)
    {
        // Arrange
        var reportDto = CreateFakeReportDto();
        reportDto.ReportId = id;

        // Act
        A.CallTo(() => _reportRepository.Get(id)).Returns(reportDto);
        var result = (OkObjectResult)await _reportController.Get(id);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ReportController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";

        // Act
        A.CallTo(() => _reportRepository.Get(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _reportController.Get(id);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ReportController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var report = CreateFakeReport();
        report.ReportId = id;

        // Act
        A.CallTo(() => _reportRepository.Delete(id)).Returns(report);
        var result = (OkObjectResult)await _reportController.Delete(id);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ReportController_Delete_ReturnsBadRequest(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";

        // Act
        A.CallTo(() => _reportRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _reportController.Delete(id);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ReportController_Update_ReturnsOk(int id)
    {
        // Arrange
        var report = CreateFakeReport();
        var reportDto = CreateFakeReportDto();
        report.ReportId = id;

        // Act
        A.CallTo(() => _reportRepository.Update(id, reportDto)).Returns(report);
        var result = (OkObjectResult)await _reportController.Update(id, reportDto);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ReportController_Update_ReturnsBadRequest(int id)
    {
        // Arrange
        var reportDto = CreateFakeReportDto();
        const string exeptionMessage = "An error occured";

        // Act
        A.CallTo(() => _reportRepository.Update(id, reportDto))
            .Throws(new SmartWMSExceptionHandler(exeptionMessage));
        var result = (BadRequestObjectResult)await _reportController.Update(id, reportDto);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ReportController_AttachFile_ReturnsOk(int id)
    {
        // Arrange
        var file = A.Fake<IFormFile>();
        var byteArr = new byte[8];
        
        // Act
        A.CallTo(() => _reportRepository.AttachFile(file, id)).Returns(byteArr);
        var result = (OkObjectResult)await _reportController.AttachFile(file, id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ReportController_AttachFile_ReturnsBadRequest(int id)
    {
        // Arrange
        var file = A.Fake<IFormFile>();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _reportRepository.AttachFile(file, id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _reportController.AttachFile(file, id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ReportController_DownloadFile_ReturnsOk(int id)
    {
        // Arrange
        var byteArr = new byte[8];
        
        // Act
        A.CallTo(() => _reportRepository.DownloadFile(id)).Returns(byteArr);
        var result = (FileContentResult)await _reportController.DownloadFile(id);
        
        // Assert
        result.FileContents.Should().BeSameAs(byteArr);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void ReportController_DownloadFile_ReturnsBadRequest(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _reportRepository.DownloadFile(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _reportController.DownloadFile(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }
}