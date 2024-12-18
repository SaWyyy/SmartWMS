using System.ComponentModel.DataAnnotations;
using SmartWMS.Entities.Enums;

namespace SmartWMS.Models.DTOs;

public class OrderHeaderDto
{
    public int? OrdersHeaderId { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime OrderDate { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime? DeliveryDate { get; set; }
    
    [RegularExpression("^([A-ZŁŚŹŻ][A-Za-zĄĆĘŁŃÓŚŹŻąćęłńóśźż]{2,}\\s){1,5}\\d{1,5}(([A-Za-z](/\\d{1,5})?)|(/\\d{1,5}))?$",
        ErrorMessage = "Bad pattern")]
    [MaxLength(65, ErrorMessage = "Address too long")]
    public string DestinationAddress { get; set; } = null!;
    
    [Range(0, 1, ErrorMessage = "Type name must be 0 or 1")]
    public OrderType TypeName { get; set; }
    
    [Range(0, 3, ErrorMessage = "Status name must be between 0 and 3")]
    public OrderName StatusName { get; set; }
}