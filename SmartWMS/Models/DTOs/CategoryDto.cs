using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartWMS.Models.DTOs;

public class CategoryDto
{
    [ReadOnly(true)]
    public int? CategoryId { get; set; }

    [MinLength(3, ErrorMessage = "Category name must have at least 3 characters")]
    [MaxLength(45,ErrorMessage = "Category Name must be 45 characters upmost")]
    public string CategoryName { get; set; }
}