using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SmartWMS.Entities.Enums;

namespace SmartWMS.Models.DTOs;

public class AlertDto
{
    public int? AlertId { get; set; }
    
    [DefaultValue(false)]
    public bool Seen { get; set; }
    
    [Required]
    [MaxLength(45)]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(45)]
    public string Description { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime AlertDate { get; set; }
    
    [Range(0, 3, ErrorMessage = "Alert type must be between 0 and 3")]
    public AlertType AlertType { get; set; }
}