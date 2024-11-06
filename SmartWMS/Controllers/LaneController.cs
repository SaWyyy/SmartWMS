using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LaneController : ControllerBase
{
    private readonly ILaneRepository _repository;
    private readonly ILogger<LaneController> _logger;

    public LaneController(ILaneRepository repository, ILogger<LaneController> logger)
    {
        this._repository = repository;
        this._logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddLane(LaneDto dto)
    {
        try
        {
            var result = await _repository.Add(dto);
            _logger.LogInformation($"Lane with id: {result.LaneId} has been added");
            
            return Ok($"Lane with id: {result.LaneId} has been added");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repository.GetAll();

        return Ok(result);
    }

    [HttpGet("getAllWithRacksShelves")]
    public async Task<IActionResult> GetAllWithRackShelves()
    {
        var result = await _repository.GetAllWithRacksShelves();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _repository.Get(id);
            _logger.LogInformation("Lane found");

            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _repository.Delete(id);
            _logger.LogInformation("Lane deleted");

            return Ok($"Lane with id: {result.LaneId} has been removed");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, LaneDto dto)
    {
        try
        {
            var result = await _repository.Update(id, dto);
            _logger.LogInformation($"Lane updated");

            return Ok($"Lane with id: {result.LaneId} has been updated");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}