using Microsoft.Extensions.Caching.Memory;

namespace SmartWMS.Entities;

public partial class Rack
{
    public int RackId { get; set; }
    public int RackNumber { get; set; }
    public int LanesLaneId { get; set; }
    public virtual Lane LaneLane { get; set; } = null!;
    public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
}