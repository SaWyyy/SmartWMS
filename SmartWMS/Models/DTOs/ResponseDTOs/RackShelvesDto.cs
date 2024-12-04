namespace SmartWMS.Models.DTOs.ResponseDTOs;

public class RackShelvesDto
{
    public int? RackId { get; set; }
    public int RackNumber { get; set; }
    public IEnumerable<ShelfDto> Shelves { get; set; } = new List<ShelfDto>();
}