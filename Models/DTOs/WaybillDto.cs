using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models;

public class WaybillDto
{

    public DateTime ShippingDate { get; set; }

    public DateTime LoadingDate { get; set; }

    public int CountriesCountryId { get; set; }

    public string PostalCode { get; set; } = null!;

    public string SupplierName { get; set; } = null!;


}