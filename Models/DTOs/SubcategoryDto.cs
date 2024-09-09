using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models;

public class SubcategoryDto
{
    [MinLength(3, ErrorMessage = "Subcategory Name must have at least 3 characters")]
    [MaxLength(45, ErrorMessage = "Subcategory Name must be 45 characters upmost")]
    public string SubcategoryName { get; set; } = null!;

    public int CategoriesCategoryId { get; set; }
}