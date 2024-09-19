using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs;

public class ProductDto
{
    public int? ProductId { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(45)]
    public string ProductName { get; set; } = null!;
    [MaxLength(45)]
    public string? ProductDescription { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
    
    [MinLength(8)]
    [MaxLength(8)]
    public string Barcode { get; set; }
    
    [Range(0, int.MaxValue)]
    public int SubcategoriesSubcategoryId { get; set; }
}