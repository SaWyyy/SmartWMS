using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartWMS;
using SmartWMS.Controllers;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMSTests.Controller;

public class UserControllerTest
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;
    private readonly UserController _userController;

    public UserControllerTest()
    {
        this._userRepository = A.Fake<IUserRepository>();
        this._logger = A.Fake<ILogger<UserController>>();
        this._userController = new UserController(_userRepository, _logger);
    }

    private static Registration CreateFakeRegistrationModel()
    {
        var model = A.Fake<Registration>();
        model.UserName = "Test";
        model.Email = "test@example.com";
        model.Password = "Test";
        return model;
    }

    private static UserDto CreateFakeUserDto()
    {
        var userDto = A.Fake<UserDto>();
        userDto.UserName = "Test";
        userDto.Email = "test@example.com";
        return userDto;
    }

    [Fact]
    public async void UserController_RegisterManager_ReturnsOk()
    {
        // Arrange
        var user = CreateFakeRegistrationModel();
        var identityResultSuccess = IdentityResult.Success;
        
        // Act
        A.CallTo(() => _userRepository.RegisterManager(user)).Returns(identityResultSuccess);
        var result = (OkObjectResult)await _userController.RegisterManager(user);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void UserController_RegisterManager_ReturnsBadRequest()
    {
        // Arrange
        var user = CreateFakeRegistrationModel();
        var identityResultFailed = IdentityResult.Failed(new IdentityError{Description = "An error occured"});
        
        // Act
        A.CallTo(() => _userRepository.RegisterManager(user)).Returns(identityResultFailed);
        var result = (BadRequestObjectResult)await _userController.RegisterManager(user);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void UserController_RegisterEmployee_ReturnsOk()
    {
        // Arrange
        var user = CreateFakeRegistrationModel();
        var identityResultSuccess = IdentityResult.Success;
        user.ManagerId = "Test";
        
        // Act
        A.CallTo(() => _userRepository.RegisterEmployee(user)).Returns(identityResultSuccess);
        var result = (OkObjectResult)await _userController.RegisterEmployee(user);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void UserController_RegisterEmployee_ReturnsBadRequest()
    {
        // Arrange
        var user = CreateFakeRegistrationModel();
        var identityResultFailed = IdentityResult.Failed(new IdentityError { Description = "An error occured" });
        
        // Act
        A.CallTo(() => _userRepository.RegisterEmployee(user)).Returns(identityResultFailed);
        var result = (BadRequestObjectResult)await _userController.RegisterEmployee(user);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("Test1")]
    [InlineData("Test2")]
    public async void UserController_GetUsersByRole_ReturnsOk(string roleName)
    {
        // Arrange
        var users = new List<UserDto>();
        users.Add(CreateFakeUserDto());
        
        // Act
        A.CallTo(() => _userRepository.GetUsers(roleName)).Returns(users);
        var result = (OkObjectResult)await _userController.GetUsersByRole(roleName);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("Test1")]
    [InlineData("Test2")]
    public async void UserController_GetUsersByRole_ReturnsNotFound(string roleName)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _userRepository.GetUsers(roleName))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _userController.GetUsersByRole(roleName);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void UserController_GetUser_ReturnsOk()
    {
        // Arrange
        var userDto = CreateFakeUserDto();
        
        // Act
        A.CallTo(() => _userRepository.GetUser()).Returns(userDto);
        var result = (OkObjectResult)await _userController.GetUser();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void UserController_GetUser_ReturnsBadRequest()
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _userRepository.GetUser())
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _userController.GetUser();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData("Test1")]
    [InlineData("Test2")]
    public async void UserController_DeleteUser_ReturnsOk(string id)
    {
        // Arrange
        var identityResultSuccess = IdentityResult.Success;
        
        // Act
        A.CallTo(() => _userRepository.DeleteUser(id)).Returns(identityResultSuccess);
        var result = (OkObjectResult)await _userController.DeleteUser(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData("Test1")]
    [InlineData("Test2")]
    public async void UserController_DeleteUser_ReturnsBadRequest(string id)
    {
        // Arrange
        var identityResultFailed = IdentityResult.Failed(new IdentityError { Description = "An error occured" });
        
        // Act
        A.CallTo(() => _userRepository.DeleteUser(id)).Returns(identityResultFailed);
        var result = (BadRequestObjectResult)await _userController.DeleteUser(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
}