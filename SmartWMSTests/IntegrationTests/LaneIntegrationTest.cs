using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMSTests.IntegrationTests.Configuration;
using Task = System.Threading.Tasks.Task;

namespace SmartWMSTests.IntegrationTests;

[Collection("Non-Parallel tests")]
public class LaneIntegrationTest : BaseIntegrationTest
{
    private readonly HttpClient _client;
    private readonly SmartwmsDbContext _dbContext;
    private readonly IServiceScope _scope;
    
    public LaneIntegrationTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
    }

    [Fact]
    public async Task AddLane_ShouldReturnOk()
    {
        // Arrange
        var laneDto = new LaneDto
        {
            LaneCode = "A1"
        };
        
        // Act
        var response = await _client.PostAsJsonAsync("/api/Lane", laneDto);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk()
    {
        // Arrange
        var lane = new Lane
        {
            LaneCode = "A2"
        };

        await _dbContext.Lanes.AddAsync(lane);
        await _dbContext.SaveChangesAsync();

        var laneId = lane.LaneId;
        
        // Act
        var response = await _client.GetAsync($"/api/Lane/{laneId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<LaneDto>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto!.LaneId.Should().Be(laneId);
        responseDto.LaneCode.Should().Be("A2");
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Arrange
        var lane = new Lane
        {
            LaneCode = "A3"
        };

        await _dbContext.Lanes.AddAsync(lane);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var response = await _client.GetAsync("/api/Lane");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<List<LaneDto>>(content, customJsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseDto.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAllWithShelves_ShouldReturnOk()
    {
        // Arrange
        var lane = new Lane
        {
            LaneCode = "A7"
        };

        await _dbContext.Lanes.AddAsync(lane);
        await _dbContext.SaveChangesAsync();

        var rack = new Rack
        {
            RackNumber = 1,
            LanesLaneId = lane.LaneId
        };

        await _dbContext.Racks.AddAsync(rack);
        await _dbContext.SaveChangesAsync();

        var shelf = new Shelf
        {
            CurrentQuant = 0,
            MaxQuant = 10,
            Level = LevelType.P0,
            RacksRackId = rack.RackId
        };

        await _dbContext.Shelves.AddAsync(shelf);
        await _dbContext.SaveChangesAsync();

        var laneId = lane.LaneId;
        
        // Act
        var response = await _client.GetAsync("/api/Lane/getAllWithRacksShelves");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var responseList = JsonSerializer.Deserialize<List<LaneRacksShelvesDto>>(content, customJsonOptions);
        var responseDto = responseList!.Find(x => x.LaneId == laneId);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseList.Should().NotBeEmpty();
        responseDto!.LaneId.Should().Be(laneId);
        responseDto.LaneCode.Should().Be("A7");
        responseDto.Racks.ToList()[0].Should().BeOfType<RackShelvesDto>();
    }
    
    [Fact]
    public async Task Update_ShouldReturnOk()
    {
        // Arrange
        var lane = new Lane
        {
            LaneCode = "A4"
        };

        await _dbContext.Lanes.AddAsync(lane);
        await _dbContext.SaveChangesAsync();

        var newLane = new LaneDto
        {
            LaneCode = "A5"
        };

        var laneId = lane.LaneId;
        
        // Act
        var response = await _client.PutAsJsonAsync($"/api/Lane/{laneId}", newLane);
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be($"Lane with id: {laneId} has been updated");
    }

    [Fact]
    public async Task Delete_ShouldReturnOk()
    {
        // Arrange
        var lane = new Lane
        {
            LaneCode = "A6"
        };

        await _dbContext.Lanes.AddAsync(lane);
        await _dbContext.SaveChangesAsync();
        
        var laneId = lane.LaneId;

        // Act
        var response = await _client.DeleteAsync($"/api/Lane/{laneId}");
        
        // Assert
        var content = await response.Content.ReadAsStringAsync();
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be($"Lane with id: {laneId} has been removed");
    }
}