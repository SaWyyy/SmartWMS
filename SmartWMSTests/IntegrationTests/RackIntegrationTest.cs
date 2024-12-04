using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.ResponseDTOs;
using SmartWMSTests.IntegrationTests.Configuration;
using Task = System.Threading.Tasks.Task;

namespace SmartWMSTests.IntegrationTests;

[Collection("Non-Parallel tests")]
public class RackIntegrationTest : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private readonly SmartwmsDbContext _dbContext;
    private readonly IServiceScope _scope;
    
    public RackIntegrationTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
    }

    private async Task<int> SeedDatabase(int laneCode)
    {
        var lane = new Lane
        {
            LaneCode = "B" + laneCode
        };

        await _dbContext.Lanes.AddAsync(lane);
        await _dbContext.SaveChangesAsync();

        return lane.LaneId;
    }

    [Fact]
    public async Task AddRack_ShouldReturnOk()
    {
        // Arrange
        var laneId = await SeedDatabase(1);

        var rack = new RackDto
        {
            RackNumber = 1,
            LanesLaneId = laneId
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/Rack", rack);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        // Arrange
        var laneId = await SeedDatabase(2);

        var rack = new Rack
        {
            RackNumber = 1,
            LanesLaneId = laneId
        };

        await _dbContext.Racks.AddAsync(rack);
        await _dbContext.SaveChangesAsync();

        var rackId = rack.RackId;
        
        // Act
        var response = await _client.GetAsync($"/api/Rack/{rackId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<RackDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.RackId.Should().Be(rackId);
        responseDto.RackNumber.Should().Be(1);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Arrange
        var laneId = await SeedDatabase(3);

        var rack = new Rack
        {
            RackNumber = 1,
            LanesLaneId = laneId
        };

        await _dbContext.Racks.AddAsync(rack);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync("/api/Rack");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDtos = JsonSerializer.Deserialize<List<RackDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDtos.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        // Arrange
        var laneId = await SeedDatabase(4);

        var rack = new Rack
        {
            RackNumber = 1,
            LanesLaneId = laneId
        };
        
        var newRack = new Rack
        {
            RackNumber = 2,
            LanesLaneId = laneId
        };

        await _dbContext.Racks.AddAsync(rack);
        await _dbContext.SaveChangesAsync();

        var rackId = rack.RackId;
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/Rack/{rackId}", newRack);
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be($"Rack with id: {rackId} has been updated");
    }

    [Fact]
    public async Task Delete_ShouldReturnOk()
    {
        //Arrange
        var laneId = await SeedDatabase(5);

        var rack = new Rack
        {
            RackNumber = 1,
            LanesLaneId = laneId
        };
        
        await _dbContext.Racks.AddAsync(rack);
        await _dbContext.SaveChangesAsync();

        var rackId = rack.RackId;
        
        // Act
        var response = await _client.DeleteAsync($"/api/Rack/{rackId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be($"Rack with id: {rackId} has been deleted");
    }

    [Fact]
    public async Task GetAllLaneRacks_ShouldReturnOk()
    {
        // Arrange
        var laneId = await SeedDatabase(6);
        
        var rack = new Rack
        {
            RackNumber = 1,
            LanesLaneId = laneId
        };
        
        await _dbContext.Racks.AddAsync(rack);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync($"/api/Rack/lanesRacks/{laneId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDtos = JsonSerializer.Deserialize<List<LanesRacksDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDtos.Should().NotBeEmpty();
        responseDtos![0].RackNumber.Should().Be(1);
    }
}