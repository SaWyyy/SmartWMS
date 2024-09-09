using SmartWMS.Entities.Enums;

namespace SmartWMS.Entities;

public partial class UsersHasTask
{
    public string UsersUserId { get; set; }
    
    public int TasksTaskId { get; set; }
    
    public ActionType Action { get; set; }

    public virtual User UsersUser { get; set; } = null!;
    
    public virtual Task TasksTask { get; set; } = null!;
}