using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs.CreateOrderDtos;

public class CreateOrderWaybillDto
{
    [Range(0, int.MaxValue)]
    public int CountryId { get; set; }
    [RegularExpression("^[0-9]{2}[0-9]{3}$", ErrorMessage = "Pattern is: XXXXX")]
    public string PostalCode { get; set; }
    
    [MinLength(3, ErrorMessage = "Supplier name must have at least 3 characters")]
    [MaxLength(45, ErrorMessage = "Supplier name is too long")]
    public string SupplierName { get; set; }
}