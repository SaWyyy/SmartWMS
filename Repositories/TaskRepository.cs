using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NLog.LayoutRenderers.Wrappers;
using SmartWMS.Models;
using SmartWMS.Models.Enums;
using Task = SmartWMS.Models.Task;

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
    
    public async Task<Task?> AddTask(TaskDto dto)
    {
        var orderHeader =
            await _dbContext.OrderHeaders.FirstOrDefaultAsync(x => x.OrdersHeaderId == dto.OrderHeadersOrdersHeaderId);

        if (orderHeader is null)
            return null;
        
        var user = _accessor.HttpContext?.User;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return null;

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

        return null;
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

    public async Task<TaskDto?> Get(int id)
    {
        var result = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);

        if (result is null)
            return null;

        return _mapper.Map<TaskDto>(result);
    }

    public async Task<Task?> Delete(int id)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);

        if (task is null)
            return null;

        var userHasTask = await _dbContext.UsersHasTasks.FirstOrDefaultAsync(x => x.TasksTaskId == task.TaskId);

        if (userHasTask is null)
            return null;
        
        _dbContext.UsersHasTasks.Remove(userHasTask);

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
        {
            _dbContext.Tasks.Remove(task);

            var result2 = await _dbContext.SaveChangesAsync();

            if (result2 > 0)
                return task;
        }

        return null;
    }

    public async Task<Task?> Update(int id, TaskDto dto)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);

        if (task is null)
            return null;

        task.Priority = dto.Priority;
        task.Seen = dto.Seen;
        task.FinishDate = dto.FinishDate;
        task.StartDate = dto.StartDate;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return task;

        return null;
    }
}