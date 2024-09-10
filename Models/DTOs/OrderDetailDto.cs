namespace SmartWMS.Models.DTOs;

public class OrderDetailDto
{
    public int Quantity { get; set; }

    public int ProductsProductId { get; set; }

    public int OrderHeadersOrdersHeaderId { get; set; }
}