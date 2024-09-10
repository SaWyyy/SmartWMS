using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs;

public class OrderDetailDto
{
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
    
    [Range(0, int.MaxValue)]
    public int ProductsProductId { get; set; }
    
    [Range(0, int.MaxValue)]
    public int OrderHeadersOrdersHeaderId { get; set; }
}