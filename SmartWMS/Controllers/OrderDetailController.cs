using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]

public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailRepository _repository;
    private readonly ILogger<OrderDetailController> _logger;

    public OrderDetailController(IOrderDetailRepository repository, ILogger<OrderDetailController> logger)
    {
        this._repository = repository;
        this._logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddOrderDetail(OrderDetailDto dto)
    {
        try
        {
            var result = await _repository.Add(dto);

            _logger.LogInformation($"Order detail with id: {result.OrderDetailId} has been added");
            return Ok($"Order detail with id: {result.OrderDetailId} has been added");
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

    [HttpGet("byOrderHeader/{orderHeaderId}")]
    public async Task<IActionResult> GetAllByOrderHeader(int orderHeaderId)
    {
        try
        {
            var result = await _repository.GetAllByOrderHeaderId(orderHeaderId);
            return Ok(result);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _repository.Get(id);

            _logger.LogInformation("Order header found");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OrderDetailDto dto)
    {
        try
        {
            var result = await _repository.Update(id, dto);

            _logger.LogInformation("Order detail updated");
            return Ok(result);
        }
        catch(SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _repository.Delete(id);

            _logger.LogInformation("Order detail deleted");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}