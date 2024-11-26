using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs.CreateOrderDtos;

public class CreateOrderOrderHeaderDto
{
    [RegularExpression("^([A-ZŁŚŹŻ][A-Za-zĄĆĘŁŃÓŚŹŻąćęłńóśźż]{2,}\\s){1,5}\\d{1,5}(([A-Za-z](/\\d{1,5})?)|(/\\d{1,5}))?$",
        ErrorMessage = "Bad pattern")]
    [MaxLength(45, ErrorMessage = "Address too long")]
    public string DestinationAddress { get; set; }
}