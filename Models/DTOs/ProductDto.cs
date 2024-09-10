using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs;

public class ProductDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(45)]
    public string ProductName { get; set; } = null!;
    [MaxLength(45)]
    public string? ProductDescription { get; set; }
    [Range(0, int.MaxValue)]
    public string Price { get; set; } = null!;
    [Range(0, int.MaxValue)]
    public int ProductDetailsProductDetailId { get; set; }
    [Range(0, int.MaxValue)]
    public int SubcategoriesSubcategoryId { get; set; }
}