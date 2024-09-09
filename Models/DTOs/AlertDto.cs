using SmartWMS.Entities.Enums;

namespace SmartWMS.Models.DTOs;

public class AlertDto
{
    public bool Seen { get; set; }

    public DateTime AlertDate { get; set; }
    
    public AlertType AlertType { get; set; }
}