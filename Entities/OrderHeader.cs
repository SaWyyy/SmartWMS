using SmartWMS.Entities.Enums;
using SmartWMS.Models;
using Task = SmartWMS.Entities.Task;

namespace SmartWMS.Entities;

public partial class OrderHeader
{
    public int OrdersHeaderId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string DestinationAddress { get; set; } = null!;
    
    public OrderType TypeName { get; set; }

    public OrderName StatusName { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    //public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual Waybill WaybillsWaybill { get; set; } = null!;
}
