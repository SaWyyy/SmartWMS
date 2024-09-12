using Microsoft.AspNetCore.Identity;
using SmartWMS.Entities;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IdentityResult> RegisterManager(Registration model);
    Task<IdentityResult> RegisterEmployee(Registration model);
    Task<IEnumerable<UserDto>> GetUsers(string roleName);
    Task<UserDto> GetUser();
}