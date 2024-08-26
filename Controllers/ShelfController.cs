using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Repositories;
using Task = System.Threading.Tasks.Task;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShelfController : ControllerBase
{
    private readonly IShelfRepository _shelfRepository;
    private readonly ILogger<ShelfController> _logger;

    public ShelfController(IShelfRepository shelfRepository, ILogger<ShelfController> logger)
    {
        this._shelfRepository = shelfRepository;
        this._logger = logger;
    }

    [HttpPost("")]
    //[Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> addShelf(CreateShelfDto dto)
    {
        var result = await _shelfRepository.AddShelf(dto);

        if (result is null)
        {
            _logger.LogError("Error has occured while adding shelf");
            return BadRequest("Error has occured while adding shelf");
        }
        
        _logger.LogInformation($"Shelf nr. {dto.Rack} has been added");
        return Ok($"Adding shelf completed, Id: {result.ShelfId}");
    }

    [HttpGet()]
    public async Task<IActionResult> getAll()
    {
        var result = await _shelfRepository.GetAll();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> get(int id)
    {
        var result = await _shelfRepository.Get(id);

        if (result is null)
        {
            _logger.LogError("Error has occured while adding shelf");
            return BadRequest("Error has occured while looking for shelf");

        }
        
        _logger.LogInformation("Shelf found");
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedShelf = await _shelfRepository.Delete(id);

        if (deletedShelf is null)
        {
            return NotFound();
        }

        _logger.LogInformation("Shelf removed");
        return Ok(deletedShelf);
    }

}