using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Models.CreateOrderDtos;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services.Interfaces;

namespace SmartWMS.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Authorize]
public class OrderHeaderController : ControllerBase
{
    private readonly IOrderHeaderRepository _repository;
    private readonly ILogger<OrderHeaderController> _logger;
    private readonly IOrderAndTasksCreationService _createOrderService;
    private readonly IOrderCancellationService _cancelOrderService;

    public OrderHeaderController(
        IOrderHeaderRepository repository, 
        ILogger<OrderHeaderController> logger, 
        IOrderAndTasksCreationService createOrderService,
        IOrderCancellationService cancelOrderService)
    {
        this._repository = repository;
        this._logger = logger;
        this._createOrderService = createOrderService;
        this._cancelOrderService = cancelOrderService;
    }

    [HttpPost("createOrder")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        try
        {
            await _createOrderService.CreateOrder(dto);
            return Ok("Order created successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("cancelOrder/{orderHeaderId}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> CancelOrder(int orderHeaderId)
    {
        try
        {
            await _cancelOrderService.CancelOrder(orderHeaderId);
            return Ok("Order cancelled successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> AddOrderHeader(OrderHeaderDto dto)
    {
        try
        {
            var result = await _repository.Add(dto);
            
            _logger.LogInformation($"OrderHeader nr. {result.OrdersHeaderId}");
            return Ok($"OrderHeader nr. {result.OrdersHeaderId}");
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

        _logger.LogInformation("All Order Headers fetched");
        return Ok(result);
    }

    [HttpGet("withDetails")]
    public async Task<IActionResult> GetAllWithDetails()
    {
        var result = await _repository.GetAllWithDetails();
        
        _logger.LogInformation("All Order Headers with Details fetched");
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
            
            _logger.LogInformation("Order Header deleted");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> Update(int id, OrderHeaderDto dto)
    {
        try
        {
            var result = await _repository.Update(id, dto);
            
            _logger.LogInformation("Order Header edited");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}