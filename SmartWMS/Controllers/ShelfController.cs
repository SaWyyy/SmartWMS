using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ShelfController : ControllerBase
{
    private readonly IShelfRepository _shelfRepository;
    private readonly ILogger<ShelfController> _logger;

    public ShelfController(IShelfRepository shelfRepository, ILogger<ShelfController> logger)
    {
        this._shelfRepository = shelfRepository;
        this._logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> AddShelf(ShelfDto dto)
    {
        try
        {
            var result = await _shelfRepository.AddShelf(dto);
            
            _logger.LogInformation($"Shelf nr. {result.ShelfId} has been added");
            return Ok($"Adding shelf completed, Id: {result.ShelfId}");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var result = await _shelfRepository.GetAll();

        return Ok(result);
    }

    [HttpGet("withRackLane")]
    public async Task<IActionResult> GetAllWithRackLane()
    {
        var result = await _shelfRepository.GetAllWithRackLanes();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _shelfRepository.Get(id);
            
            _logger.LogInformation("Shelf found");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deletedShelf = await _shelfRepository.Delete(id);

            _logger.LogInformation("Shelf removed");
            return Ok(deletedShelf);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            _logger.LogError(e.Message);
            return Conflict(e.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Update(int id, ShelfDto dto)
    {
        try
        {
            var updatedShelf = await _shelfRepository.Update(id, dto);
            
            _logger.LogInformation("Shelf edited");
            return Ok(updatedShelf);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("racksLevels/{id}")]
    public async Task<IActionResult> GetAllRacksLevels(int id)
    {
        try
        {
            var result = await _shelfRepository.GetAllRacksLevels(id);
            _logger.LogInformation(("Rack's levels have been fetched successfully"));
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }
}