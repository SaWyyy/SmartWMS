using SmartWMS.Models;

namespace SmartWMS.Entities;

public partial class Task
{
    public int TaskId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? FinishDate { get; set; }

    public int Priority { get; set; }

    public bool? Seen { get; set; }
    
    public int QuantityCollected { get; set; }
    
    public int QuantityAllocated { get; set; }

    public bool Done { get; set; }
    
    public int OrderDetailsOrderDetailId { get; set; }

    public virtual OrderDetail OrderDetailsOrderDetail { get; set; } = null!;

    public virtual ICollection<UsersHasTask> UsersHasTasks { get; set; } = new List<UsersHasTask>();
}
