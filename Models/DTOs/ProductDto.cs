namespace SmartWMS.Models.DTOs;

public class ProductDto
{
    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public string Price { get; set; } = null!;
    
    public int ProductDetailsProductDetailId { get; set; }

    public int SubcategoriesSubcategoryId { get; set; }
}