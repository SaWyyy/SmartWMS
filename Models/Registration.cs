using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models;

public class Registration
{
    [EmailAddress]
    public string Email { get; set; }
    [MinLength(5)]
    public string UserName { get; set; }
    
    [DataType(DataType.Password)]
    [Required]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[#?!@$%^&*-]).{8,}$", ErrorMessage = 
        "Password must have minimum 8 characters and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    
    public string Password { get; set; }
    
    public string? ManagerId { get; set; }
}