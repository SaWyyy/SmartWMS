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
    public async Task<IActionResult> addTask(TaskDto dto)
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
    public async Task<IActionResult> getAll(ActionType? type)
    {
        var result = await _repository.GetAll(type);
        
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> get(int id)
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
    public async Task<IActionResult> delete(int id)
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
    public async Task<IActionResult> update(int id, TaskDto dto)
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
}