using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Repositories;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductDetailController: ControllerBase
{
    private readonly IProductDetailRepository _repository;
    private readonly ILogger<ProductDetailController> _logger;

    public ProductDetailController(IProductDetailRepository repository, ILogger<ProductDetailController> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(ProductDetailDto dto)
    {
        try
        {
            var result = await _repository.Add(dto);

            _logger.LogInformation($"ProductDetail nr. {result.ProductDetailId}");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repository.GetAll();
        _logger.LogInformation("All Product Details fetched");
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _repository.Get(id);
            _logger.LogInformation("Product Detail found");
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
            
            _logger.LogInformation("Product Detail deleted");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ProductDetailDto dto)
    {
        try
        {
            var result = await _repository.Update(id, dto);
            
            _logger.LogInformation("Product Detail edited");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}