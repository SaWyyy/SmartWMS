using Castle.Core.Logging;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartWMS;
using SmartWMS.Controllers;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.ReturnEnums;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services;
using SmartWMS.Services.Interfaces;
using ILogger = Castle.Core.Logging.ILogger;
using Task = SmartWMS.Entities.Task;

namespace SmartWMSTests.Controller;

public class TaskControllerTest
{
    private readonly ITaskRepository _taskRepository; 
    private readonly ILogger<TaskController> _logger;
    private readonly IOrderValidationService _service;
    private readonly TaskController _taskController;

    public TaskControllerTest()
    {
        this._taskRepository = A.Fake<ITaskRepository>();
        this._logger = A.Fake<ILogger<TaskController>>();
        this._service = A.Fake<IOrderValidationService>();
        this._taskController = new TaskController(_taskRepository, _service, _logger);
    }

    private Task CreateFakeTask()
    {
        var task = A.Fake<Task>();
        task.TaskId = 1;
        task.Priority = 1;
        task.UsersHasTasks = new List<UsersHasTask>();
        task.Seen = false;
        task.FinishDate = new DateTime();
        task.QuantityAllocated = 1;
        task.QuantityCollected = 1;
        task.StartDate = new DateTime();
        task.OrderDetailsOrderDetail = new OrderDetail();
        task.OrderDetailsOrderDetailId = 1;
        return task;
    }

    private TaskDto CreateFakeTaskDto()
    {
        var taskDto = A.Fake<TaskDto>();
        taskDto.TaskId = 1;
        taskDto.Priority = 1;
        taskDto.FinishDate = new DateTime();
        taskDto.FinishDate = new DateTime();
        taskDto.Seen = false;
        taskDto.QuantityAllocated = 1;
        taskDto.QuantityCollected = 1;
        taskDto.OrderDetailsOrderDetailId = 1;
        return taskDto;
    }

    [Fact]
    public async void TaskController_Add_ReturnsOk()
    {
        // Arrange
        var task = CreateFakeTask();
        var taskDto = CreateFakeTaskDto();
        
        // Act
        A.CallTo(() => _taskRepository.AddTask(taskDto)).Returns(task);
        var result = (OkObjectResult)await _taskController.AddTask(taskDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void TaskController_Add_ReturnsBadRequest()
    {
        // Arrange
        var taskDto = CreateFakeTaskDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _taskRepository.AddTask(taskDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _taskController.AddTask(taskDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData(ActionType.Taken)]
    [InlineData(ActionType.Given)]
    public async void TaskController_GetAll_ReturnsOk(ActionType? type)
    {
        // Arrange
        var tasks = new List<TaskDto>();
        tasks.Add(CreateFakeTaskDto());
        
        // Act
        A.CallTo(() => _taskRepository.GetAll(type)).Returns(tasks);
        var result = (OkObjectResult)await _taskController.GetAll(type);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void TaskController_Get_ReturnsOk(int id)
    {
        // Arrange
        var taskDto = CreateFakeTaskDto();
        
        // Act
        A.CallTo(() => _taskRepository.Get(id)).Returns(taskDto);
        var result = (OkObjectResult)await _taskController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void TaskController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _taskRepository.Get(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _taskController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void TaskController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var task = CreateFakeTask();
        task.TaskId = id;
        
        // Act
        A.CallTo(() => _taskRepository.Delete(id)).Returns(task);
        var result = (OkObjectResult)await _taskController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void TaskController_Delete_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _taskRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _taskController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void TaskController_Update_ReturnsOk(int id)
    {
        // Arrange
        var task = CreateFakeTask();
        var taskDto = CreateFakeTaskDto();
        task.TaskId = id;
        
        // Act
        A.CallTo(() => _taskRepository.Update(id, taskDto)).Returns(task);
        var result = (OkObjectResult)await _taskController.Update(id, taskDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void TaskController_Update_ReturnsBadRequest(int id)
    {
        // Arrange
        var taskDto = CreateFakeTaskDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _taskRepository.Update(id, taskDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _taskController.Update(id, taskDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void TaskController_TakeTask_ReturnsOk(int id)
    {
        // Arrange
        var taskDto = CreateFakeTaskDto();
        
        // Act
        A.CallTo(() => _taskRepository.TakeTask(id)).Returns(taskDto);
        var result = (OkObjectResult)await _taskController.TakeTask(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void TaskController_TakeTask_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _taskRepository.TakeTask(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _taskController.TakeTask(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void TaskController_GetUserTasks_ReturnsOk()
    {
        // Arrange
        var tasks = new List<TaskDto>();
        tasks.Add(CreateFakeTaskDto());
        
        // Act
        A.CallTo(() => _taskRepository.UserTasks()).Returns(tasks);
        var result = (OkObjectResult)await _taskController.GetUserTasks();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void TaskController_GetUserTasks_ReturnsNotFound()
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _taskRepository.UserTasks())
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _taskController.GetUserTasks();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void TaskController_UpdateQuantity_ReturnsOk()
    {
        // Arrange
        int id = 1;
        
        // Act
        A.CallTo(() => _service.CheckOrderCompletion(id)).Returns(OrderValidation.TaskFinished);
        var result = (OkObjectResult)await _taskController.UpdateQuantity(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }
}