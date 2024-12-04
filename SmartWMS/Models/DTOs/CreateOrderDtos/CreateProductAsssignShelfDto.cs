namespace SmartWMS.Models.DTOs.CreateOrderDtos;

public class CreateProductAsssignShelfDto
{
    public ProductDto ProductDto { get; set; }
    public IEnumerable<ShelfDto> Shelves { get; set; } = new List<ShelfDto>();
}