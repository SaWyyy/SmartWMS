using System.Net.Http.Json;
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
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Alert with id: 1 has been added");
    }
}