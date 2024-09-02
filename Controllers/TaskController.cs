using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Models.Enums;
using SmartWMS.Repositories;
using Task = SmartWMS.Models.Task;

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
        var result = await _repository.AddTask(dto);

        if (result is null)
        {
            _logger.LogError("Error has occured while adding task");
            return BadRequest("Error has occured while adding task");
        }
        
        _logger.LogInformation($"Task nr. {result.TaskId} has been added");
        return Ok($"Task nr. {result.TaskId} has been added");
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
        var result = await _repository.Get(id);

        if (result is null)
        {
            _logger.LogError("Error has occured while looking for task");
            return NotFound("Error has occured while looking for task");
        }
        
        _logger.LogInformation("Task found");
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repository.Delete(id);

        if (result is null)
        {
            _logger.LogError("Task with specified ID hasn't been found");
            return NotFound("Task with specified ID hasn't been found");
        }
        
        _logger.LogInformation("Task removed");
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskDto dto)
    {
        var result = await _repository.Update(id, dto);

        if (result is null)
        {
            _logger.LogError("Task with specified ID hasn't been edited");
            return BadRequest("Error has occured while editing waybill");
        }
        
        _logger.LogInformation("Task edited");
        return Ok(result);
    }

    [Authorize(Roles = "Employee")]
    [HttpPost("take/{id}")]
    public async Task<IActionResult> TakeTask(int id)
    {
        var result = await _repository.TakeTask(id);
        if (result is null)
        {
            _logger.LogError("Unable to find task with given id");
            return NotFound("Task with specified ID hasn't been found");
        }
        
        _logger.LogInformation("User has taken task with specified ID");
        return Ok(result);
    }

    [HttpGet("usertasks")]
    [Authorize(Roles = "Employee,Manager")]
    public async Task<IActionResult> GetUserTasks()
    {
        var result = await _repository.userTasks();
        if (result is null)
        {
            _logger.LogError("Cannot find tasks for currently logged user");
            return NotFound("Cannot find tasks for user");
        }
        
        _logger.LogInformation("User's tasks fetched successfully");
        return Ok(result);
    }
}