namespace SmartWMS.Models.DTOs.ResponseDTOs;

public class RackLaneDto
{
    public int RackId { get; set; }
    public int RackNumber { get; set; }
    public LaneDto Lane { get; set; } = null!;
}