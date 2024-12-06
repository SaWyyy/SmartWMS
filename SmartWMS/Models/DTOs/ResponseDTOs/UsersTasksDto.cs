namespace SmartWMS.Models.DTOs.ResponseDTOs;

public class UsersTasksDto
{
    public required string UserId { get; set; }
    public required string Username { get; set; }
    public IEnumerable<TaskDto> Tasks { get; set; } = new List<TaskDto>();
}