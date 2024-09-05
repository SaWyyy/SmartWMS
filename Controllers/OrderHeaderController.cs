using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Repositories;

namespace SmartWMS.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class OrderHeaderController : ControllerBase
{
    private readonly IOrderHeaderRepository _repository;
    private readonly ILogger<OrderHeaderController> _logger;

    public OrderHeaderController(IOrderHeaderRepository repository, ILogger<OrderHeaderController> logger)
    {
        this._repository = repository;
        this._logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddOrderHeader(OrderHeaderDto dto)
    {
        try
        {
            var result = await _repository.Add(dto);
            
            _logger.LogInformation($"OrderHeader nr. {result.OrdersHeaderId}");
            return Ok($"OrderHeader nr. {result.OrdersHeaderId}");
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
        var result = await _repository.GetAll();

        _logger.LogInformation("All Order Headers fetched");
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _repository.Get(id);
            
            _logger.LogInformation("Order Header Found");
            return Ok(result);
        }
        catch (Exception e)
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
            
            _logger.LogInformation("Order Header deleted");
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OrderHeaderDto dto)
    {
        try
        {
            var result = await _repository.Update(id, dto);
            
            _logger.LogInformation("Order Header edited");
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}