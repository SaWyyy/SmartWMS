using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs;

public class TaskDto
{
    [DataType(DataType.DateTime)]
    public DateTime? StartDate { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime? FinishDate { get; set; }
    
    [Range(0, 5, ErrorMessage = "Priority is integer between 0 and 5")]
    public int Priority { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Must be integer value grater than 0")]
    public int OrderDetailsOrderDetailId { get; set; }
    [DefaultValue(false)]
    public bool? Seen { get; set; }
    
    public int QuantityCollected { get; set; }
    
    public int QuantityAllocated { get; set; }
}