using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Repositories;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WaybillController : ControllerBase
{
    private readonly IWaybillRepository _waybillRepository;
    private readonly ILogger<WaybillController> _logger;

    public WaybillController(IWaybillRepository waybillRepository, ILogger<WaybillController> logger)
    {
        _waybillRepository = waybillRepository;
        _logger = logger;
    }

    [HttpPost("")]
    public async Task<IActionResult> addWaybill(WaybillDto dto)
    {
        var result = await _waybillRepository.AddWaybill(dto);
        
        if (result is null)
        {
            _logger.LogError("Error has occured while creating waybill");
            return BadRequest("Error has occured while adding waybill");
        }
        
        _logger.LogInformation($"Waybill nr. {result.WaybillId}");
        return Ok($"Adding waybill completed, Id: {result.WaybillId}");
    }

    [HttpGet()]
    public async Task<IActionResult> getAll()
    {
        var result = await _waybillRepository.GetAll();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> get(int id)
    {
        var result = await _waybillRepository.Get(id);

        if (result is null)
        {
            _logger.LogError("Error has occured while looking for waybill");
            return BadRequest("Error has occured while looking for waybill");
        }
        
        _logger.LogInformation("Waybill found");
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> delete(int id)
    {
        var deletedWaybill = await _waybillRepository.Delete(id);

        if (deletedWaybill is null)
        {
            _logger.LogError("Waybill with specified ID hasnt been found");
            return NotFound();
        }
        
        _logger.LogInformation("Waybill removed");
        return Ok(deletedWaybill);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> update(int id, WaybillDto dto)
    {
        var updatedWaybill = await _waybillRepository.Update(id, dto);

        if (updatedWaybill is null)
        {
            _logger.LogError("Waybill with specified ID hasnt been edited");
            return BadRequest("Error has occured while editing waybill");
        }
        
        _logger.LogInformation("Waybill edited");
        return Ok(updatedWaybill);
    }
}