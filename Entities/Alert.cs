using SmartWMS.Entities.Enums;
using SmartWMS.Models;

namespace SmartWMS.Entities;

public partial class Alert
{
    public int AlertId { get; set; }

    public bool Seen { get; set; }

    public DateTime AlertDate { get; set; }

    public int WarehousesWarehouseId { get; set; }
    
    public AlertType AlertType { get; set; }
    public virtual Warehouse WarehousesWarehouse { get; set; } = null!;
}
