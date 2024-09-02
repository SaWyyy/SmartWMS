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
        var result = await _repository.Add(dto);

        if (result is null)
        {
            _logger.LogError("Error has occured while creating order header");
            return BadRequest("Error has occured while creating order header");
        }
        
        _logger.LogInformation($"OrderHeader nr. {result.OrdersHeaderId}");
        return Ok($"OrderHeader nr. {result.OrdersHeaderId}");
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
        var result = await _repository.Get(id);

        if (result is null)
        {
            _logger.LogError("Error has occured while looking for Order Header");
            return NotFound("Error has occured while looking for Order Header");
        }
        
        _logger.LogInformation("Order Header Found");
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repository.Delete(id);

        if (result is null)
        {
            _logger.LogError("Order Header with specified ID hasn't been found");
            return NotFound("Order Header with specified ID hasn't been found");
        }
        
        _logger.LogInformation("Order header deleted");
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OrderHeaderDto dto)
    {
        var result = await _repository.Update(id, dto);

        if (result is null)
        {
            _logger.LogError("Order Header with specified ID hasn't been edited");
            return BadRequest("Error has occured while editing waybill");
        }
        
        _logger.LogInformation("Waybill edited");
        return Ok(result);
    }
}