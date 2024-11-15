using System.ComponentModel.DataAnnotations;
using SmartWMS.Entities.Enums;

namespace SmartWMS.Models.DTOs;

public class ShelfDto
{
    public int? ShelfId { get; set; }

    [Range(0, 4, ErrorMessage = "Level must be between 0 and 4")]
    public LevelType Level { get; set; }
    
    [Range(0, int.MaxValue, MinimumIsExclusive = false, ErrorMessage = "Quantity cant be less than 1")]
    public int MaxQuant { get; set; }

    [Range(0, int.MaxValue, MinimumIsExclusive = false, ErrorMessage = "Quantity cant be less than 0")]
    public int CurrentQuant { get; set; }

    public int? ProductsProductId { get; set; }
    
    public int RacksRackId { get; set; }
}