namespace SmartWMS.Entities;

public partial class OrderShelvesAllocation
{
    public int OrderShelvesAllocationId { get; set; }
    public int ProductId { get; set; }
    public int ShelfId { get; set; }
    public int  TaskId { get; set; }
    public int Quantity { get; set; }
}