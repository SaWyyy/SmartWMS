using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs;

public class CategoryDto
{
    [MinLength(3, ErrorMessage = "Category name must have at least 3 characters")]
    [MaxLength(45,ErrorMessage = "Category Name must be 45 characters upmost")]
    public string CategoryName { get; set; }
}