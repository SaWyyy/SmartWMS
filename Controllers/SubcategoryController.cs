using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Repositories;

namespace SmartWMS.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class SubcategoryController : ControllerBase
{
    private readonly ISubcategoryRepository _repository;
    private readonly ILogger<SubcategoryController> _logger;

    public SubcategoryController(ILogger<SubcategoryController> logger, ISubcategoryRepository repository)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Add(SubcategoryDto dto)
    {
        try
        {
            var result = await _repository.Add(dto);

            _logger.LogInformation($"Subcategory {result.SubcategoryId} has been added");
            return Ok($"Adding subcategory completed, Id: {result.SubcategoryId}");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _repository.Get(id);

            _logger.LogInformation("Subcategory found");
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _repository.GetAll());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _repository.Delete(id);
            
            _logger.LogInformation("Subcategory removed");
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SubcategoryDto dto)
    {
        try
        {
            var result = await _repository.Update(id, dto);
            
            _logger.LogInformation("Subcategory edited");
            return Ok("Subcategory edited");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}