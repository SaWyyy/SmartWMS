using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models;

public class CountryDto
{
    [StringLength(40, ErrorMessage = "Country name is to long")]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Country name should contain only letters")]
    public string CountryName { get; set; } = null!;

    [RegularExpression("^\\d{1,2}$", ErrorMessage = "Country code should have one or two digit.")]
    public int CountryCode { get; set; }

}