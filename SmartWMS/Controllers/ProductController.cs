using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductController> _logger;
    
    public ProductController(IProductRepository repository, ILogger<ProductController> logger)
    {
        this._repository = repository;
        this._logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Add(ProductDto dto)
    {
        try
        {
            var result = await _repository.Add(dto);
            
            _logger.LogInformation($"Product with id: {result.ProductId} has been added");
            return Ok($"Product with id: {result.ProductId} has been added");
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

            _logger.LogInformation("Product found");
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

            _logger.LogInformation("Product deleted");
            return Ok($"Product with id: {result.ProductId} has been deleted");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductDto dto)
    {
        try
        {
            var result = await _repository.Update(id, dto);

            _logger.LogInformation("Product updated");
            return Ok($"Product with id: {result.ProductId} has been updated");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}