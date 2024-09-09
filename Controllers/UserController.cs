using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Models;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace SmartWMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;
    public UserController(IUserRepository userRepository, ILogger<UserController> logger)
    {
        this._userRepository = userRepository;
        this._logger = logger;
    }
    
    [HttpPost("register/manager")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterManager(Registration model)
    {
        var result = await _userRepository.RegisterManager(model);

        if (!result.Succeeded)
        {
            _logger.LogError($"Error has occured while registering {model.UserName}.");
            return BadRequest(result.Errors);
        }
            
        _logger.LogInformation($"{model.UserName} has been registered.");
        return Ok("Registration successful");
    }
    
    [HttpPost("register/employee")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterEmployee(Registration model)
    {
        var result = await _userRepository.RegisterEmployee(model);

        if (!result.Succeeded)
        {
            _logger.LogError($"Error has occured while registering {model.UserName}.");
            return BadRequest(result.Errors);
        }
        
        _logger.LogInformation($"{model.UserName} has been registered.");
        return Ok("Registration successful");
    }
}
