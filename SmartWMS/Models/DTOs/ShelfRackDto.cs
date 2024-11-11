using SmartWMS.Entities.Enums;

namespace SmartWMS.Models.DTOs;

public class ShelfRackDto
{
    public int ShelfId { get; set; }
    public LevelType Level { get; set; }
    public int MaxQuant { get; set; }
    public int CurrentQuant { get; set; }
    public RackLaneDto RackLane { get; set; } = null!;
}