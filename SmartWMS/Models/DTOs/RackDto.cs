using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs;

public class RackDto
{
    public int? RackId { get; set; }
    
    [Range(0, 20, MinimumIsExclusive = false, ErrorMessage = "Rack number must be between 0 and 20")]
    public int RackNumber { get; set; }
    
    [Range(0, int.MaxValue)]
    public int LanesLaneId { get; set; }
}