using System.ComponentModel.DataAnnotations;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;

namespace SmartWMS.Models.DTOs;

public class CreateShelfDto
{
    [RegularExpression("^[A-D][0-9]{1,2}$", ErrorMessage = "Lane must contain capital letter and number, 2 digit at most.")]
    public string Lane { get; set; } = null!;

    [Range(1, int.MaxValue, MinimumIsExclusive = false, ErrorMessage = "Rack numeration must start from 1")]
    public int Rack { get; set; }

    [Range(0, 4, ErrorMessage = "Level must be between 0 and 4")]
    public LevelType Level { get; set; }
    
    [Range(0, int.MaxValue, MinimumIsExclusive = true, ErrorMessage = "Quantity cant be less than 1")]
    public int MaxQuant { get; set; }

    [Range(0, int.MaxValue, MinimumIsExclusive = false, ErrorMessage = "Quantity cant be less than 0")]
    public int CurrentQuant { get; set; }

    public int? ProductsProductId { get; set; }

    public virtual Product? ProductsProduct { get; set; }
}