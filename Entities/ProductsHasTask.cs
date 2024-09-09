using Task = SmartWMS.Entities.Task;

namespace SmartWMS.Entities;

public partial class ProductsHasTask
{
    public int ProductsProductId { get; set; }

    public int TasksTaskId { get; set; }

    public int QuantityAllocated { get; set; }

    public int? QuantityCollected { get; set; }

    public virtual Product ProductsProduct { get; set; } = null!;

    public virtual Task TasksTask { get; set; } = null!;
}
