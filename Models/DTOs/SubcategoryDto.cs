using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models;

public class SubcategoryDto
{
    [MaxLength(45, ErrorMessage = "Subcategory Name must be 45 characters upmost")]
    public string SubcategoryName { get; set; } = null!;

    public int CategoriesCategoryId { get; set; }
}