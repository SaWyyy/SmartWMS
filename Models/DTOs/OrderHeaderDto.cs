using System.ComponentModel.DataAnnotations;
using SmartWMS.Models.Enums;

namespace SmartWMS.Models;

public class OrderHeaderDto
{
    [DataType(DataType.DateTime)]
    public DateTime OrderDate { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime? DeliveryDate { get; set; }
    
    [RegularExpression("^([A-ZŁŚŹŻ][A-Za-zĄĆĘŁŃÓŚŹŻąćęłńóśźż]{2,}\\s){1,5}\\d{1,5}([A-Za-z]|(/\\d{1,5}))?$",
        ErrorMessage = "Bad pattern")]
    [MaxLength(45, ErrorMessage = "Address too long")]
    public string DestinationAddress { get; set; } = null!;
    
    [Range(1, int.MaxValue, ErrorMessage = "Must be integer value grater than 0")]
    public int WaybillsWaybillId { get; set; }
    
    [Range(0, 1, ErrorMessage = "Type name must be 0 or 1")]
    public OrderType TypeName { get; set; }
    
    [Range(0, 3, ErrorMessage = "Status name must be between 0 and 3")]
    public OrderName StatusName { get; set; }
}