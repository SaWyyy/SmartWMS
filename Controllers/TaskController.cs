using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Entities.Enums;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;
using Task = SmartWMS.Entities.Task;

namespace SmartWMS.Controllers;

[Route("/api/[controller]")]
[ApiController]

public class TaskController : ControllerBase
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<TaskController> _logger;

    public TaskController(ITaskRepository repository, ILogger<TaskController> logger)
    {
        this._logger = logger;
        this._repository = repository;
    }

    [Authorize(Roles = "Manager")]
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
}