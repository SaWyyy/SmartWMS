using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Authorize]
public class AlertController : ControllerBase
{
    private readonly IAlertRepository _repository;
    private readonly ILogger<AlertController> _logger;
    
    public AlertController(IAlertRepository repository, ILogger<AlertController> logger)
    {
        this._repository = repository;
        this._logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddAlert(AlertDto dto)
    {
        try
        {
            var result = await _repository.Add(dto);
            
            _logger.LogInformation($"Alert with id: {result.AlertId} has been added");
            return Ok($"Alert with id: {result.AlertId} has been added");
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
            
            _logger.LogInformation("Alert found");
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

            _logger.LogInformation("Alert removed");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, AlertDto dto)
    {
        try
        {
            var result = await _repository.Update(id, dto);

            _logger.LogInformation("Alert updated");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("seen/{id}")]
    public async Task<IActionResult> ChangeSeen(int id)
    {
        try
        {
            var result = await _repository.ChangeSeen(id);

            _logger.LogInformation("Alert seen");
            return Ok(result);
        }
        catch(SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}