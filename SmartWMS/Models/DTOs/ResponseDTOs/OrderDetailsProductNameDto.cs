namespace SmartWMS.Models.DTOs.ResponseDTOs;

public class OrderDetailsProductNameDto
{
    public int? OrderDetailId { get; set; }
    public int Quantity { get; set; }
    public int ProductsProductId { get; set; }
    public required string ProductName { get; set; }
    public int OrderHeadersOrdersHeaderId { get; set; }
    public bool Done { get; set; }
}