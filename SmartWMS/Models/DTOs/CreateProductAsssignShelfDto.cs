namespace SmartWMS.Models.DTOs;

public class CreateProductAsssignShelfDto
{
    public ProductDto ProductDto { get; set; }
    public IEnumerable<ShelfDto> Shelves { get; set; } = new List<ShelfDto>();
}