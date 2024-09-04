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

    [HttpPost]
    //[Authorize(Roles = "Manager, Admin")]
    public async Task<IActionResult> AddShelf(ShelfDto dto)
    {
        try
        {
            var result = await _shelfRepository.AddShelf(dto);
            
            _logger.LogInformation($"Shelf nr. {result.ShelfId} has been added");
            return Ok($"Adding shelf completed, Id: {result.ShelfId}");
        }
        catch (Exception e)
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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _shelfRepository.Get(id);
            
            _logger.LogInformation("Shelf found");
            return Ok(result);
        }
        catch (Exception e)
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
            var deletedShelf = await _shelfRepository.Delete(id);
            
            _logger.LogInformation("Shelf removed");
            return Ok(deletedShelf);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ShelfDto dto)
    {
        try
        {
            var updatedShelf = await _shelfRepository.Update(id, dto);
            
            _logger.LogInformation("Shelf edited");
            return Ok(updatedShelf);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}