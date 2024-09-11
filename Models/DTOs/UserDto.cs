using System.ComponentModel.DataAnnotations;

namespace SmartWMS.Models.DTOs;

public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string ManagerId { get; set; }
}