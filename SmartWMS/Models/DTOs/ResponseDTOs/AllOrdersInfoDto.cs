using SmartWMS.Entities.Enums;

namespace SmartWMS.Models.DTOs.ResponseDTOs;

public class AllOrdersInfoDto
{
    public int OrdersHeaderId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public required string DestinationAddress { get; set; }
    public OrderType TypeName { get; set; }
    public OrderName StatusName { get; set; }
    public IEnumerable<OrderDetailsProductNameDto> OrderDetails { get; set; } = new List<OrderDetailsProductNameDto>();
}