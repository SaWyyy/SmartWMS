using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.ReturnEnums;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services;
using SmartWMS.Services.Interfaces;
using Xunit.Abstractions;
using TaskEntity = SmartWMS.Entities.Task;
using Task = System.Threading.Tasks.Task;

namespace SmartWMSTests.Services;

public class OrderValidationServiceTest
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IOrderValidationService _service;
    private readonly ITaskRepository _taskRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IOrderHeaderRepository _orderHeaderRepository;
    private ITestOutputHelper _helper;

    public OrderValidationServiceTest(ITestOutputHelper helper)
    {
        var options = new DbContextOptionsBuilder<SmartwmsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
            
        this._dbContext = new SmartwmsDbContext(options);
        this._taskRepository = A.Fake<ITaskRepository>();
        this._orderDetailRepository = A.Fake<IOrderDetailRepository>();
        this._orderHeaderRepository = A.Fake<IOrderHeaderRepository>();
        this._helper = helper;
        this._service = new OrderValidationService(
            _dbContext,
            _taskRepository,
            _orderDetailRepository,
            _orderHeaderRepository
        );
    }

    
    private static OrderHeader CreateFakeOrderHeader()
    {
        var orderHeader = new OrderHeader
        {
            OrdersHeaderId = 1,
            DestinationAddress = "Test",
            StatusName = OrderName.Planned,
        };

        return orderHeader;
    }

    private static OrderDetail CreateFakeOrderDetail(int id, OrderHeader orderHeader)
    {
        var orderDetail = new OrderDetail
        {
            OrderDetailId = id,
            OrderHeadersOrdersHeaderId = orderHeader.OrdersHeaderId,
            OrderHeadersOrdersHeader = orderHeader,
            ProductsProductId = 1,
            Quantity = 1
        };

        return orderDetail;
    }
    
    private static TaskEntity CreateFakeTask(int id, OrderDetail orderDetail)
    {
        var task = new TaskEntity
        {
            TaskId = id,
            OrderDetailsOrderDetailId = orderDetail.OrderDetailId,
            QuantityCollected = 5,
            QuantityAllocated = 5,
            Priority = 5
        };

        return task;
    }


    private async Task CreateFakeDataBase()
    {
        var orderHeader = CreateFakeOrderHeader();
        var orderDetails = new List<OrderDetail>
        {
            CreateFakeOrderDetail(1, orderHeader),
            CreateFakeOrderDetail(2, orderHeader)
        };
    
        var tasks = new List<TaskEntity>
        {
            CreateFakeTask(1, orderDetails[0]),
            CreateFakeTask(2, orderDetails[0]),
            CreateFakeTask(3, orderDetails[1]),
            CreateFakeTask(4, orderDetails[1])
        };
        
        await _dbContext.OrderHeaders.AddAsync(orderHeader);
        await _dbContext.OrderDetails.AddRangeAsync(orderDetails);
        await _dbContext.Tasks.AddRangeAsync(tasks);
        
        await _dbContext.SaveChangesAsync();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async void CheckOrderCompletion_OrderHeaderCompleted_ReturnsOrderHeaderFinished(int id)
    {
        // Arrange
        await CreateFakeDataBase();
        
        var task = await _dbContext.Tasks
            .IgnoreQueryFilters()
            .Include(od => od.OrderDetailsOrderDetail)
            .FirstOrDefaultAsync(x => x.TaskId == id);
        
        var orderDetail = task!.OrderDetailsOrderDetail;

        A.CallTo(() => _taskRepository.UpdateQuantity(id)).Returns(CreateFakeTask(id, orderDetail));
        A.CallTo(() => _orderDetailRepository.CheckTasksForOrderDetail(orderDetail.OrderDetailId)).Returns(true);
        A.CallTo(() => _orderHeaderRepository.CheckOrderDetailsForOrderHeader(orderDetail.OrderHeadersOrdersHeaderId)).Returns(true);
        
        // Act
        var result = await _service.CheckOrderCompletion(id);

        // Assert
        result.Should().Be(OrderValidation.OrderHeaderFinished);
    }
    

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task CheckOrderCompletion_TaskCompleted_CompletesOrderDetailWhenAllTasksCompleted(int id)
    {
        // Arrange
        await CreateFakeDataBase();
        
        var task = await _dbContext.Tasks
            .IgnoreQueryFilters()
            .Include(od => od.OrderDetailsOrderDetail)
            .FirstOrDefaultAsync(x => x.TaskId == id);
        
        var orderDetail = task!.OrderDetailsOrderDetail;

        A.CallTo(() => _taskRepository.UpdateQuantity(id)).Returns(CreateFakeTask(id, orderDetail));
        A.CallTo(() => _orderDetailRepository.CheckTasksForOrderDetail(orderDetail.OrderDetailId)).Returns(true);
        
        // Act
        var result = await _service.CheckOrderCompletion(id);
        var orderDetailresult = await _dbContext.OrderDetails.IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.OrderDetailId == 1);
        
        // Assert
        result.Should().Be(OrderValidation.OrderDetailFinished);
        orderDetailresult!.Done.Should().Be(true);
    }
    
    [Fact]
    public async void CheckOrderCompletion_TaskNotCompleted_ReturnsTaskNotFinished()
    {
        // Arrange
        var taskId = 1;
        var task = new TaskEntity { TaskId = taskId, QuantityCollected = 5, QuantityAllocated = 10, Done = false };

        A.CallTo(() => _taskRepository.UpdateQuantity(taskId)).Returns(task);

        // Act
        var result = await _service.CheckOrderCompletion(taskId);

        // Assert
        result.Should().Be(OrderValidation.TaskNotFinished);
    }
}