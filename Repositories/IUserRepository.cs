using Microsoft.AspNetCore.Identity;
using SmartWMS.Models;

namespace SmartWMS.Repositories;

public interface IUserRepository
{
    Task<IdentityResult> RegisterManager(Registration model);
    Task<IdentityResult> RegisterEmployee(Registration model);
}