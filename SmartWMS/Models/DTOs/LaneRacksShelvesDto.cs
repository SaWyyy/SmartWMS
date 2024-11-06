namespace SmartWMS.Models.DTOs;

public class LaneRacksShelvesDto
{
    public int? LaneId { get; set; }
    public string LaneCode { get; set; }
    public IEnumerable<RackShelvesDto> Racks { get; set; } = new List<RackShelvesDto>();
}