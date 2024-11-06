using SmartWMS.Entities.Enums;

namespace SmartWMS.Entities;

public partial class Shelf
{

    public int ShelfId { get; set; }

    public LevelType Level { get; set; }

    public int MaxQuant { get; set; }

    public int CurrentQuant { get; set; }

    public int? ProductsProductId { get; set; }
    
    public int RacksRackId { get; set; }

    public virtual Rack RackRack { get; set; } = null!;

    public virtual Product? ProductsProduct { get; set; }
}
 