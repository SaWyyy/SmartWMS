using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs;

public class LaneDto
{
    public int? LaneId { get; set; }
    
    [RegularExpression("^[A-D][0-9]{1,2}$", ErrorMessage = "Lane must contain capital letter and number, 2 digit at most.")]
    public string LaneCode { get; set; }
}