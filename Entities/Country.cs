using SmartWMS.Models;

namespace SmartWMS.Entities;

public partial class Country
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public int CountryCode { get; set; }

    public virtual ICollection<Waybill> Waybills { get; set; } = new List<Waybill>();
}
