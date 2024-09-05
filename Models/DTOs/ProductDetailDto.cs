using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models;

public class ProductDetailDto
{
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
    
    [MinLength(8)]
    [MaxLength(8)]
    [Range(int.MinValue,int.MaxValue, ErrorMessage = "Only integers are permitted")]
    public string Barcode { get; set; } = null!;
}