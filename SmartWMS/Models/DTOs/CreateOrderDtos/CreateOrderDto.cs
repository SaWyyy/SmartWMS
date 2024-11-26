using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SmartWMS.Models.DTOs.CreateOrderDtos;

namespace SmartWMS.Models.CreateOrderDtos;

public class CreateOrderDto
{
    public CreateOrderOrderHeaderDto OrderHeader { get; set; }
    public CreateOrderWaybillDto Waybill { get; set; }
    public IEnumerable<CreateOrderProductDto> Products { get; set; } = new List<CreateOrderProductDto>();
}