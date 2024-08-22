using System;
using System.Collections.Generic;

namespace SmartWMS.Models;

public partial class Waybill
{
    public int WaybillId { get; set; }

    public DateTime ShippingDate { get; set; }

    public DateTime LoadingDate { get; set; }

    public int CountriesCountryId { get; set; }

    public string PostalCode { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public virtual Country CountriesCountry { get; set; } = null!;

    public virtual ICollection<OrderHeader> OrderHeaders { get; set; } = new List<OrderHeader>();
}
