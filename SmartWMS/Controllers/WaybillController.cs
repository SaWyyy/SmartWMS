using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WaybillController : ControllerBase
{
    private readonly IWaybillRepository _waybillRepository;
    private readonly ILogger<WaybillController> _logger;

    public WaybillController(IWaybillRepository waybillRepository, ILogger<WaybillController> logger)
    {
        _waybillRepository = waybillRepository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddWaybill(WaybillDto dto)
    {
        try
        {
            var result = await _waybillRepository.AddWaybill(dto);
            
            _logger.LogInformation($"Waybill nr. {result.WaybillId}");
            return Ok($"Adding waybill completed, Id: {result.WaybillId}");
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
        var result = await _waybillRepository.GetAll();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _waybillRepository.Get(id);

            _logger.LogInformation("Waybill found");
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
            var deletedWaybill = await _waybillRepository.Delete(id);

            _logger.LogInformation("Waybill removed");
            return Ok(deletedWaybill);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, WaybillDto dto)
    {
        try
        {
            var updatedWaybill = await _waybillRepository.Update(id, dto);
            
            _logger.LogInformation("Waybill edited");
            return Ok(updatedWaybill);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);   
        }
    }
}