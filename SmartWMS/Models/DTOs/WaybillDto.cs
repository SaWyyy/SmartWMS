using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs;

public class WaybillDto
{
    public int? WaybillId { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime ShippingDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime LoadingDate { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Must be integer value grater than 0")]
    public int CountriesCountryId { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Must be integer value grater than 0")]
    public int OrderHeadersOrderHeaderId { get; set; }
    
    [RegularExpression("^[0-9]{2}-[0-9]{3}$", ErrorMessage = "Pattern is: XX-XXX")]
    public string PostalCode { get; set; } = null!;
    
    [MinLength(3, ErrorMessage = "Supplier name must have at least 3 characters")]
    [MaxLength(45, ErrorMessage = "Supplier name is too long")]
    public string SupplierName { get; set; } = null!;
    
    [MinLength(8)]
    [MaxLength(14)]
    public string Barcode { get; set; } = null!;
}