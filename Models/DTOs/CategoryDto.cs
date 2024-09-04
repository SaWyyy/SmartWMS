using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models;

public class CategoryDto
{
    [MaxLength(45,ErrorMessage = "Category Name must be 45 characters upmost")]
    public string CategoryName { get; set; }
}