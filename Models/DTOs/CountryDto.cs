using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs;

public class CountryDto
{
    [Required(ErrorMessage = "Country name is required")]
    [RegularExpression("^[a-zA-Z]{2,40}$", ErrorMessage = "Country name should contain only letters and its length should range from 4 to 40")]
    public string CountryName { get; set; } = null!;

    [RegularExpression("^[1-9]{1}\\d?", ErrorMessage = "Country code should have one or two digit.")]
    public int CountryCode { get; set; }

}