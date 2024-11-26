using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs.CreateOrderDtos;

public class CreateOrderProductDto
{
    [Range(0, int.MaxValue)]
    public int ProductId { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}