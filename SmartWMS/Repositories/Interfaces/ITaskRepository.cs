using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.DTOs.ResponseDTOs;
using SmartWMS.Models.ReturnEnums;
using Task = SmartWMS.Entities.Task;


namespace SmartWMS.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<Task> AddTask(TaskDto dto);
    Task<IEnumerable<TaskDto>> GetAll(ActionType? type);
    Task<TaskDto> Get(int id);
    Task<TaskDto> GetByOrderDetailId(int orderDetailId);
    Task<OrderInfoDto> GetTaskItems(int id);
    Task<Task> Delete(int id);
    Task<Task> Update(int id, TaskDto dto);
    Task<TaskDto> TakeTask(int id);
    Task<IEnumerable<TaskDto>> UserTasks();
    Task<IEnumerable<UsersTasksDto>> GetAllUsersWithTasks();
    Task<Task> UpdateQuantity(int id);
}