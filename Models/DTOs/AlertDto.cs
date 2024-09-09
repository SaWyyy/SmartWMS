using SmartWMS.Models.Enums;

namespace SmartWMS.Models;

public class AlertDto
{
    public bool Seen { get; set; }

    public DateTime AlertDate { get; set; }
    
    public AlertType AlertType { get; set; }
}