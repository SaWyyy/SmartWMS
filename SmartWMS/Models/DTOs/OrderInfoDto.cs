namespace SmartWMS.Models.DTOs;

public class OrderInfoDto
{
    public required string ProductName { get; set; }
    public required string Barcode { get; set; }
    public int QuantityAll { get; set; }
    public IEnumerable<ShelfRackDto> Shelves { get; set; } = new List<ShelfRackDto>();
}