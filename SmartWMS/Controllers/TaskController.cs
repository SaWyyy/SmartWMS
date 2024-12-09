using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Entities.Enums;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services;
using SmartWMS.Services.Interfaces;
using Task = SmartWMS.Entities.Task;

namespace SmartWMS.Controllers;

[Route("/api/[controller]")]
[ApiController]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _repository;
    private readonly IOrderValidationService _service;
    private readonly ILogger<TaskController> _logger;

    public TaskController(ITaskRepository repository, IOrderValidationService service, ILogger<TaskController> logger)
    {
        this._logger = logger;
        this._repository = repository;
        this._service = service;
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost]
    public async Task<IActionResult> AddTask(TaskDto dto)
    {
        try
        {
            var result = await _repository.AddTask(dto);
            
            _logger.LogInformation($"Task nr. {result.TaskId} has been added");
            return Ok($"Task nr. {result.TaskId} has been added");
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(ActionType? type)
    {
        var result = await _repository.GetAll(type);
        
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _repository.Get(id);
            
            _logger.LogInformation("Task found");
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
            
            _logger.LogInformation("Task removed");
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
    public async Task<IActionResult> Update(int id, TaskDto dto)
    {
        try
        {
            var result = await _repository.Update(id, dto);
            
            _logger.LogInformation("Task edited");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [Authorize(Roles = "Employee")]
    [HttpPost("take/{id}")]
    public async Task<IActionResult> TakeTask(int id)
    {
        try
        {
            var result = await _repository.TakeTask(id);
            
            _logger.LogInformation("User has taken task with specified ID");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet("usertasks")]
    [Authorize(Roles = "Employee,Manager")]
    public async Task<IActionResult> GetUserTasks()
    {
        try
        {
            var result = await _repository.UserTasks();
            
            _logger.LogInformation("User's tasks fetched successfully");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpGet("usersFinishedTasks")]
    public async Task<IActionResult> GetUsersWithFinishedTasks()
    {
        var result = await _repository.GetAllUsersWithTasks();

        return Ok(result);
    }

    [HttpGet("orderInfo/{id}")]
    public async Task<IActionResult> GetOrderInfo(int id)
    {
        try
        {
            var result = await _repository.GetTaskItems(id);
            
            _logger.LogInformation("Order info fetched successfully");
            return Ok(result);
        }
        catch (SmartWMSExceptionHandler e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("UpdateQuantity/{id}")]
    public async Task<IActionResult> UpdateQuantity(int id)
    {
        var result = await _service.CheckOrderCompletion(id);
        return Ok(result);
    }
}