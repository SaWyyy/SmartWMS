using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models;

public class WaybillDto
{
    [DataType(DataType.DateTime)]
    public DateTime ShippingDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime LoadingDate { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Must be integer value grater than 0")]
    public int CountriesCountryId { get; set; }
    
    [RegularExpression("^[0-9]{2}-[0-9]{3}\n", ErrorMessage = "Pattern is: XX-XXX")]
    public string PostalCode { get; set; } = null!;
    
    [MaxLength(45, ErrorMessage = "Supplier name is too long")]
    public string SupplierName { get; set; } = null!;


}