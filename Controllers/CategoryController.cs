using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryRepository categoryRepository, ILogger<CategoryController> logger)
    {
        this._categoryRepository = categoryRepository;
        this._logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryDto dto)
    {
        try
        {
            var result = await _categoryRepository.AddCategory(dto);
            
            _logger.LogInformation($"Category {dto.CategoryName} has been added");
            return Ok($"Adding category completed, Id: {result.CategoryId}");
        }
        catch (SmartWMSExceptionHandler e)
        {
           _logger.LogError(e.Message);
           return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        try
        {
            var result = await _categoryRepository.GetCategory(id);
            _logger.LogInformation("Category found");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _categoryRepository.GetAll();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _categoryRepository.Delete(id);
            _logger.LogInformation("Category removed");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CategoryDto dto)
    {
        try
        {
            var updatedCategory = await _categoryRepository.Update(id, dto);

            _logger.LogInformation("Category edited");
            return Ok(updatedCategory);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}