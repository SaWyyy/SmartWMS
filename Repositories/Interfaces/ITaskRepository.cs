using SmartWMS.Models;
using SmartWMS.Models.Enums;
using Task = SmartWMS.Models.Task;


namespace SmartWMS.Repositories;

public interface ITaskRepository
{
    Task<Task> AddTask(TaskDto dto);
    Task<IEnumerable<TaskDto>> GetAll(ActionType? type);
    Task<TaskDto> Get(int id);
    Task<Task> Delete(int id);
    Task<Task> Update(int id, TaskDto dto);
    Task<TaskDto> TakeTask(int id);
    Task<IEnumerable<TaskDto>> UserTasks();
}