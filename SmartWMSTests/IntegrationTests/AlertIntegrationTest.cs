using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMSTests.IntegrationTests.Configuration;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace SmartWMSTests.IntegrationTests;

public class AlertIntegrationTest : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private readonly SmartwmsDbContext _dbContext;
    private readonly IServiceScope _scope;
    public AlertIntegrationTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
    }

    [Fact]
    public async Task AddAlert_ShouldReturnOk()
    {
        // Arrange
        var alertDto = new AlertDto()
        {
            Title = "Test alert",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryDelay,
            Seen = false
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/Alert", alertDto);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    
    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        // Arrange
        var alert = new Alert
        {
            Title = "Test alert getId",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryDelay,
            Seen = false,
            WarehousesWarehouseId = 1
        };
        await _dbContext.Alerts.AddAsync(alert);
        await _dbContext.SaveChangesAsync();

        var alertId = alert.AlertId;
        
        // Act
        var response = await _client.GetAsync($"/api/Alert/{alertId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<AlertDto>(content, customJsonOptions);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto.Should().NotBeNull();
        responseDto!.AlertId.Should().Be(alertId);
        responseDto.Title.Should().Be("Test alert getId");
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Arrange
        var alert = new Alert
        {
            Title = "Test alert getAll",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryDelay,
            Seen = false,
            WarehousesWarehouseId = 1
        };
        await _dbContext.Alerts.AddAsync(alert);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync("/api/Alert");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var alertDtos = JsonSerializer.Deserialize<List<AlertDto>>(content, customJsonOptions);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        alertDtos.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        // Arrange
        var alert = new Alert
        {
            Title = "Test alert update",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryDelay,
            Seen = false,
            WarehousesWarehouseId = 1
        };
        
        var newAlert = new Alert
        {
            Title = "Test alert updated",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryCanceled,
            Seen = false,
            WarehousesWarehouseId = 1
        };
        
        await _dbContext.Alerts.AddAsync(alert);
        await _dbContext.SaveChangesAsync();
        
        var alertId = alert.AlertId;
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/Alert/{alertId}", newAlert);
        
        // Arrange
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<AlertDto>(content, customJsonOptions);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.AlertId.Should().Be(alertId);
        responseDto.Title.Should().Be("Test alert updated");
        responseDto.AlertType.Should().Be(AlertType.DeliveryCanceled);
    }

    [Fact]
    public async Task ChangeSeen_ShouldReturnOk()
    {
        // Arrange
        var alert = new Alert()
        {
            Title = "Test alert changeSeen",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryDelay,
            Seen = false,
            WarehousesWarehouseId = 1
        };
        
        await _dbContext.Alerts.AddAsync(alert);
        await _dbContext.SaveChangesAsync();

        var alertId = alert.AlertId;
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/Alert/seen/{alertId}", alertId);
        
        // Arrange
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<AlertDto>(content, customJsonOptions);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.AlertId.Should().Be(alertId);
        responseDto.Title.Should().Be("Test alert changeSeen");
        responseDto.Seen.Should().Be(true);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk()
    {
        // Arrange
        var alert = new Alert()
        {
            Title = "Test alert delete",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryDelay,
            Seen = false,
            WarehousesWarehouseId = 1
        };
        
        await _dbContext.Alerts.AddAsync(alert);
        await _dbContext.SaveChangesAsync();

        var alertId = alert.AlertId;
        
        // Act
        var response = await _client.DeleteAsync($"/api/Alert/{alertId}");
        
        // Arrange
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<AlertDto>(content, customJsonOptions);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.AlertId.Should().Be(alertId);
        responseDto.Title.Should().Be("Test alert delete");
    }
}