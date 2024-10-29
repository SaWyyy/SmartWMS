using System.Data.SqlTypes;
using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NLog.LayoutRenderers.Wrappers;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.ReturnEnums;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services;
using Task = SmartWMS.Entities.Task;

namespace SmartWMS.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _accessor;

    public TaskRepository(SmartwmsDbContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
        this._accessor = accessor;
    }
    
    public async Task<Task> AddTask(TaskDto dto)
    {
        dto.TaskId = null;
        dto.Done = false;
        var orderDetail =
            await _dbContext.OrderDetails.FirstOrDefaultAsync(x => 
                x.OrderDetailId == dto.OrderDetailsOrderDetailId);

        if (orderDetail is null)
            throw new SmartWMSExceptionHandler("OrderDetail with specified id hasn't been found");

        var tasks = await _dbContext.Tasks.Where(e => e.OrderDetailsOrderDetailId == orderDetail.OrderDetailId).ToListAsync();

        var summedQuantity = tasks.Sum(e => e.QuantityAllocated);

        if (summedQuantity >= orderDetail.Quantity || summedQuantity+dto.QuantityAllocated > orderDetail.Quantity)
            throw new SmartWMSExceptionHandler("Cannot assign new task to specified order detail");
        
        var user = _accessor.HttpContext?.User;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            throw new SmartWMSExceptionHandler("User id not found");

        var task = _mapper.Map<Task>(dto);

        await _dbContext.Tasks.AddAsync(task);

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
        {
            var userHasTask = new UsersHasTask
            {
                UsersUserId = userId!,
                TasksTaskId = task.TaskId,
                Action = ActionType.Given
            };

            await _dbContext.UsersHasTasks.AddAsync(userHasTask);

            var result2 = await _dbContext.SaveChangesAsync();

            if (result2 > 0)
                return task;

            _dbContext.Remove(task);

            await _dbContext.SaveChangesAsync();
        }

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to task table");
    }

    public async Task<IEnumerable<TaskDto>> GetAll(ActionType? type)
    {
        var result = new List<Task>();
        
        switch (type)
        {
            case null:
                result = await _dbContext.Tasks.ToListAsync();
                break;
            
            case ActionType.Given:
                result = await _dbContext.Tasks
                    .Where(task => _dbContext.UsersHasTasks
                        .Any(ut => ut.TasksTaskId == task.TaskId && ut.Action == ActionType.Given))
                    .ToListAsync();
                break;
            
            case ActionType.Taken:
                result = await _dbContext.Tasks
                    .Where(task => _dbContext.UsersHasTasks
                        .Any(ut => ut.TasksTaskId == task.TaskId && ut.Action == ActionType.Taken))
                    .ToListAsync();
                break;
        }
        
        return _mapper.Map<List<TaskDto>>(result);
    }

    public async Task<TaskDto> Get(int id)
    {
        var result = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);

        if (result is null)
            throw new SmartWMSExceptionHandler("Task with specified id hasn't been found");

        return _mapper.Map<TaskDto>(result);
    }

    public async Task<Task> Delete(int id)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);

        if (task is null)
            throw new SmartWMSExceptionHandler("Task with specified id hasn't been found");

        var userHasTask = await _dbContext.UsersHasTasks.FirstOrDefaultAsync(x => x.TasksTaskId == task.TaskId);

        if (userHasTask is null)
            throw new SmartWMSExceptionHandler("Constraint violation: Relation between task and user not found");
        
        _dbContext.UsersHasTasks.Remove(userHasTask);

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
        {
            _dbContext.Tasks.Remove(task);

            var result2 = await _dbContext.SaveChangesAsync();

            if (result2 > 0)
                return task;
        }

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to task table");
    }

    public async Task<Task> Update(int id, TaskDto dto)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);

        if (task is null)
            throw new SmartWMSExceptionHandler("Task with specified id hasn't been found");

        task.Priority = dto.Priority;
        task.Seen = dto.Seen;
        task.FinishDate = dto.FinishDate;
        task.StartDate = dto.StartDate;
        task.QuantityAllocated = dto.QuantityAllocated;
        task.QuantityCollected = dto.QuantityCollected;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return task;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to task table");
    }

    public async Task<TaskDto> TakeTask(int id)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);

        if (task is null)
            throw new SmartWMSExceptionHandler("Task with specified id hasn't been found");

        var duplicateTask =
            await _dbContext.UsersHasTasks.FirstOrDefaultAsync(x =>
                x.TasksTaskId == id && x.Action == ActionType.Taken);
        
        if (duplicateTask is not null)
            throw new SmartWMSExceptionHandler("Task with specified id is assigned to another user");
        
        var user = _accessor.HttpContext?.User;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            throw new SmartWMSExceptionHandler("User id not found");

        var userHasTask = new UsersHasTask
        {
            UsersUserId = userId!,
            TasksTaskId = task.TaskId,
            Action = ActionType.Taken
        };
        
        await _dbContext.UsersHasTasks.AddAsync(userHasTask);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
        {
            var taskDto = _mapper.Map<TaskDto>(task);
            return taskDto;
        }
        
        throw new SmartWMSExceptionHandler("Error has occured while saving changes to task table");
    }

    public async Task<IEnumerable<TaskDto>> UserTasks()
    {
        var user = _accessor.HttpContext?.User;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            throw new SmartWMSExceptionHandler("User id not found");

        var userHasTasks =
            await _dbContext.Tasks
                .Where(task => _dbContext.UsersHasTasks
                    .Any(ut => ut.UsersUserId == userId && ut.Action == ActionType.Taken))
                .ToListAsync();
        
        if (!userHasTasks.Any())
            throw new SmartWMSExceptionHandler("User has no tasks");

        var tasks = _mapper.Map<List<TaskDto>>(userHasTasks);
        return tasks;
    }

    public async Task<Task> UpdateQuantity(int id)
    {
        var task = await _dbContext.Tasks
            .FirstOrDefaultAsync(x => x.TaskId == id);

        if (task is null)
            throw new SmartWMSExceptionHandler("Task not found");

        task.QuantityCollected++;
        await _dbContext.SaveChangesAsync();

        return task;
    }
}