using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.CreateOrderDtos;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services.Interfaces;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly IProductAssignmentService _service;
    private readonly ILogger<ProductController> _logger;
    
    public ProductController(IProductRepository repository, IProductAssignmentService service, ILogger<ProductController> logger)
    {
        this._repository = repository;
        this._service = service;
        this._logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
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

    [HttpPost("createAndAssignToShelves")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> CreateAndAssignToShelves(CreateProductAsssignShelfDto dto)
    {
        try
        {
            await _service.CreateAndAssignProductToShelves(dto);
            _logger.LogInformation("Product created and assigned successfully");
            return Ok("Product created and assigned successfully");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("takeDeliveryAndDistribute")]
    public async Task<IActionResult> AssignProductForDelivery(CreateProductAsssignShelfDto dto)
    {
        try
        {
            await _service.AssignProductForDelivery(dto);
            _logger.LogInformation("Product assigned for delivery");
            return Ok("Product assigned for delivery");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
        catch (ConflictException e)
        {
            _logger.LogError(e.Message);
            return Conflict(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repository.GetAll();
        return Ok(result);
    }

    [HttpGet("quantityGtZero")]
    public async Task<IActionResult> GetAllWithQuantityGtZero()
    {
        var result = await _repository.GetAllWithQuantityGtZero();
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

    [HttpGet("byBarcode/{barcode}")]
    public async Task<IActionResult> GetByBarcode(string barcode)
    {
        try
        {
            var result = await _repository.GetByBarcode(barcode);
            
            _logger.LogInformation("Product found by barcode");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("withShelves/{id}")]
    public async Task<IActionResult> GetWithShelves(int id)
    {
        try
        {
            var result = await _repository.GetWithShelves(id);
            
            _logger.LogInformation("Product found");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        { 
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet("withShelves")]
    public async Task<IActionResult> GetAllWithShelves()
    {
        try
        {
            var result = await _repository.GetAllWithShelves();
            
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
    [Authorize(Roles = "Admin, Manager")]
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
        catch (ConflictException e)
        {
            _logger.LogError(e.Message);
            return Conflict(e.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize("Admin, Manager")]
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