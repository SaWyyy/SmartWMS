using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs;

public class SubcategoryDto
{
    [MinLength(3, ErrorMessage = "Subcategory Name must have at least 3 characters")]
    [MaxLength(45, ErrorMessage = "Subcategory Name must be 45 characters upmost")]
    public string SubcategoryName { get; set; } = null!;
    
    [Range(0, int.MaxValue)]
    public int CategoriesCategoryId { get; set; }
}