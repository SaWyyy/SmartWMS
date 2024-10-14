using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class ReportController: ControllerBase
{
    private readonly IReportRepository _repository;
    private readonly ILogger<ReportController> _logger;

    public ReportController(IReportRepository repository, ILogger<ReportController> logger)
    {
        this._repository = repository;
        this._logger = logger;
    }

    [HttpPut("uploadFile")]
    public async Task<IActionResult> AttachFile(IFormFile file, int id)
    {
        try
        {
            var result = await _repository.AttachFile(file, id);

            _logger.LogInformation("File uploaded successfully");
            return Ok($"File uploaded to report with id: {id}");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(ReportDto dto)
    {
        try
        {
            var result = await _repository.Add(dto);
            
            _logger.LogInformation("Report added successfully");
            return Ok($"Report with id: {result.ReportId} has been added successfully");
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
            
            _logger.LogInformation("Report found");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _repository.Delete(id);
            
            _logger.LogInformation("Report deleted");
            return Ok($"Report with id: {result.ReportId} has been deleted");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ReportDto dto)
    {
        try
        {
            var result = await _repository.Update(id, dto);
            
            _logger.LogInformation("Report updated");
            return Ok($"Report with id: {result.ReportId} has been updated");
        }
        catch(SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("downloadFile/{id}")]
    public async Task<IActionResult> DownloadFile(int id)
    {
        try
        {
            var result = await _repository.DownloadFile(id);
            _logger.LogInformation("Downloading requested file");

            return File(result, "application/pdf");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }
}