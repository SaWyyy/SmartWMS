using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMSTests.IntegrationTests.Configuration;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace SmartWMSTests.IntegrationTests;

[Collection("Non-Parallel tests")]
public class ReportIntegrationTest : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private readonly SmartwmsDbContext _dbContext;
    private readonly IServiceScope _scope;
    private readonly ITestOutputHelper _helper;
    
    public ReportIntegrationTest(IntegrationTestWebAppFactory factory, ITestOutputHelper helper) : base(factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
        _helper = helper;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    private byte[] CreateSamplePdf(string title, string content)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Content().Column(column =>
                {
                    column.Spacing(10);
                    column.Item().Text(title).FontSize(20).Bold();
                    column.Item().Text(content).FontSize(12);
                });
            });
        }).GeneratePdf();

        return document;
    }

    [Fact]
    public async Task AddReport_ShouldReturnOk()
    {
        // Arrange
        var report = new ReportDto
        {
            ReportDate = DateTime.Now,
            ReportPeriod = ReportPeriod.Day,
            ReportType = ReportType.Deliveries
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/Report", report);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        // Arrange
        var report = new Report
        {
            ReportDate = DateTime.Now,
            ReportPeriod = ReportPeriod.Day,
            ReportType = ReportType.Deliveries,
            ReportFile = new byte[]{0x0},
            WarehousesWarehouseId = 1
        };

        await _dbContext.Reports.AddAsync(report);
        await _dbContext.SaveChangesAsync();

        var reportId = report.ReportId;
        
        // Act
        var response = await _client.GetAsync($"/api/Report/{reportId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ReportDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.ReportId.Should().Be(reportId);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Arrange
        var report = new Report
        {
            ReportDate = DateTime.Now,
            ReportPeriod = ReportPeriod.Day,
            ReportType = ReportType.Deliveries,
            ReportFile = new byte[]{0x0},
            WarehousesWarehouseId = 1
        };

        await _dbContext.Reports.AddAsync(report);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync($"/api/Report");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<List<ReportDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        // Arrange
        var report = new Report
        {
            ReportDate = DateTime.Now,
            ReportPeriod = ReportPeriod.Day,
            ReportType = ReportType.Deliveries,
            ReportFile = new byte[]{0x0},
            WarehousesWarehouseId = 1
        };

        await _dbContext.Reports.AddAsync(report);
        await _dbContext.SaveChangesAsync();

        var reportId = report.ReportId;
        
        var reportNew = new ReportDto
        {
            ReportDate = DateTime.Now,
            ReportPeriod = ReportPeriod.Day,
            ReportType = ReportType.Shipments
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/Report/{reportId}", reportNew);
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be($"Report with id: {reportId} has been updated");
    }
    
    [Fact]
    public async Task Delete_ShouldReturnOk()
    {
        // Arrange
        var report = new Report
        {
            ReportDate = DateTime.Now,
            ReportPeriod = ReportPeriod.Day,
            ReportType = ReportType.Deliveries,
            ReportFile = new byte[]{0x0},
            WarehousesWarehouseId = 1
        };

        await _dbContext.Reports.AddAsync(report);
        await _dbContext.SaveChangesAsync();

        var reportId = report.ReportId;
        
        // Act
        var response = await _client.DeleteAsync($"/api/Report/{reportId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be($"Report with id: {reportId} has been deleted");
    }

    [Fact]
    public async Task AttachFile_ShouldReturnOk()
    {
        // Arrange
        var report = new Report
        {
            ReportDate = DateTime.Now,
            ReportPeriod = ReportPeriod.Day,
            ReportType = ReportType.Deliveries,
            ReportFile = new byte[]{0x0},
            WarehousesWarehouseId = 1
        };

        await _dbContext.Reports.AddAsync(report);
        await _dbContext.SaveChangesAsync();

        var reportId = report.ReportId;

        var pdfBytes = CreateSamplePdf("Test PDF", "Test content");
        var fileContent = new MultipartFormDataContent
        {
            { new ByteArrayContent(pdfBytes), "file", "test.pdf" }
        };
        
        // Act
        var response = await _client.PutAsync($"/api/Report/uploadFile/{reportId}", fileContent);
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be($"File uploaded to report with id: {reportId}");
    }
    
    [Fact]
    public async Task DownloadFile_ShouldReturnOk()
    {
        // Arrange
        var pdfBytes = CreateSamplePdf("Test PDF", "Test content");
        
        var report = new Report
        {
            ReportDate = DateTime.Now,
            ReportPeriod = ReportPeriod.Day,
            ReportType = ReportType.Deliveries,
            ReportFile = pdfBytes,
            WarehousesWarehouseId = 1
        };

        await _dbContext.Reports.AddAsync(report);
        await _dbContext.SaveChangesAsync();

        var reportId = report.ReportId;
        
        // Act
        var response = await _client.GetAsync($"/api/Report/downloadFile/{reportId}");
        
        // Assert
        var content = await response.Content.ReadAsByteArrayAsync();
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/pdf");
        response.Content.Headers.ContentDisposition?.FileName.Should().Be($"report-{reportId}.pdf");
        content.Should().NotBeEmpty();
    }
}