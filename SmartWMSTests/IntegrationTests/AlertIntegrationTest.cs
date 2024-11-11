using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMSTests.IntegrationTests.Configuration;
using Xunit.Abstractions;

namespace SmartWMSTests.IntegrationTests;

public class AlertIntegrationTest : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private ITestOutputHelper _helper;
    public AlertIntegrationTest(IntegrationTestWebAppFactory factory, ITestOutputHelper helper) : base(factory)
    {
        _client = factory.CreateClient();
        this._helper = helper;
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
        int alertId = 1;
        var alertDto = new AlertDto()
        {
            Title = "Test alert",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryDelay,
            Seen = false
        };
        await _client.PostAsJsonAsync("/api/Alert", alertDto);
        
        // Act
        var response = await _client.GetAsync($"/api/Alert/{alertId}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<AlertDto>(content, customJsonOptions);
        responseDto.Should().NotBeNull();
        responseDto!.AlertId.Should().Be(alertId);
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Arrange
        int alertId = 1;
        var alertDto = new AlertDto()
        {
            Title = "Test alert",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryDelay,
            Seen = false
        };
        await _client.PostAsJsonAsync("/api/Alert", alertDto);
        
        // Act
        var response = await _client.GetAsync("/api/Alert");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var alertDtos = JsonSerializer.Deserialize<List<AlertDto>>(content, customJsonOptions);
        alertDtos.Should().NotBeEmpty();
        alertDtos![0].AlertId.Should().Be(alertId);
    }

    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        // Arrange
        int alertId = 1;
        var alertDto = new AlertDto()
        {
            Title = "Test alert",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryDelay,
            Seen = false
        };
        await _client.PostAsJsonAsync("/api/Alert", alertDto);
        var newAlertDto = new AlertDto
        {
            Title = "Test alert",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryCanceled,
            Seen = false
        };
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/Alert/{alertId}", newAlertDto);
        
        // Arrange
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<AlertDto>(content, customJsonOptions);
        responseDto!.AlertId.Should().Be(alertId);
        responseDto.AlertType.Should().Be(AlertType.DeliveryCanceled);
    }

    [Fact]
    public async Task ChangeSeen_ShouldReturnOk()
    {
        // Arrange
        int alertId = 1;
        var alertDto = new AlertDto()
        {
            Title = "Test alert",
            Description = "Test description",
            AlertDate = DateTime.Now,
            AlertType = AlertType.DeliveryDelay,
            Seen = false
        };
        await _client.PostAsJsonAsync("/api/Alert", alertDto);
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/Alert/seen/{alertId}", alertId);
        
        // Arrange
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<AlertDto>(content, customJsonOptions);
        responseDto!.AlertId.Should().Be(alertId);
        responseDto.Seen.Should().Be(true);
    }
}