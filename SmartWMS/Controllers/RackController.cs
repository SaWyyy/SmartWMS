using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RackController : ControllerBase
{
    private readonly IRackRepository _repository;
    private readonly ILogger<RackController> _logger;

    public RackController(IRackRepository repository, ILogger<RackController> logger)
    {
        this._repository = repository;
        this._logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddRack(RackDto dto)
    {
        try
        {
            var result = await _repository.Add(dto);
            _logger.LogInformation($"Rack with id: {result.RackId} has been added");

            return Ok($"Rack with id: {result.RackId} has been added");
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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _repository.Get(id);
            _logger.LogInformation("Rack found");

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
            _logger.LogInformation("Rack deleted");

            return Ok($"Rack with id: {result.RackId} has been deleted");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, RackDto dto)
    {
        try
        {
            var result = await _repository.Update(id, dto);
            _logger.LogInformation("Rack updated");

            return Ok($"Rack with id: {result.RackId} has been deleted");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("lanesRacks/{id}")]
    public async Task<IActionResult> GetAllLanesRacks(int id)
    {
        try
        {
            var result = await _repository.GetAllLanesRacks(id);
            _logger.LogInformation("Lane's racks have been fetched successfully");
            
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }
}