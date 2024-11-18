using SmartWMS.Entities;

namespace SmartWMS.Models.DTOs;

public class ProductShelfDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string? ProductDescription { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Barcode { get; set; }
    public int SubcategoryId { get; set; }
    public IEnumerable<ShelfRackDto> Shelves { get; set; } = new List<ShelfRackDto>();
}