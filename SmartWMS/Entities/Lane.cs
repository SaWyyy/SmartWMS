namespace SmartWMS.Entities;

public partial class Lane
{
    public int LaneId { get; set; }
    public string LaneCode { get; set; }
    public virtual ICollection<Rack> Racks { get; set; } = new List<Rack>();
}