using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Repositories;

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
        
        _logger.LogInformation($"Shlef nr. {dto.Rack} has been added");
        return Ok("Adding shelf completed");
        


    }
}