using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMS.Models.ReturnEnums;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace SmartWMS.Services;

public class OrderValidationService : IOrderValidationService
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly ITaskRepository _taskRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IOrderHeaderRepository _orderHeaderRepository;

    public OrderValidationService(
        SmartwmsDbContext dbContext,
        ITaskRepository taskRepository,
        IOrderDetailRepository orderDetailRepository,
        IOrderHeaderRepository orderHeaderRepository)
    {
        this._dbContext = dbContext;
        this._taskRepository = taskRepository;
        this._orderDetailRepository = orderDetailRepository;
        this._orderHeaderRepository = orderHeaderRepository;
    }

    public async Task<OrderValidation> CheckOrderCompletion(int taskId)
    {
        var task = await _taskRepository.UpdateQuantity(taskId);
        if (task.QuantityCollected == task.QuantityAllocated)
        {
            task.Done = true;
            await _dbContext.SaveChangesAsync();
            var result = await CheckTasksForOrderDetail(task.OrderDetailsOrderDetailId);
            
            return result;
        }
        
        return OrderValidation.TaskNotFinished;
    }
    
    public async Task<OrderValidation> CheckTasksForOrderDetail(int orderDetailId)
    {
        var orderDetail = await _dbContext.OrderDetails
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.OrderDetailId == orderDetailId);
        var tasksCompleted = await _orderDetailRepository.CheckTasksForOrderDetail(orderDetailId);

        if (orderDetail is null)
            throw new SmartWMSExceptionHandler("OrderDetail not found");
        
        if (tasksCompleted)
        {
            orderDetail.Done = true;
            await _dbContext.SaveChangesAsync();

            var result = await CheckOrderDetailsForOrderHeader(orderDetail.OrderHeadersOrdersHeaderId);
            if (result)
                return OrderValidation.OrderHeaderFinished;
                
            return OrderValidation.OrderDetailFinished;
        }

        return OrderValidation.TaskFinished;
    }

    public async Task<bool> CheckOrderDetailsForOrderHeader(int orderHeaderId)
    {
        try
        {
            var orderHeader = await _orderHeaderRepository.Get(orderHeaderId);
            var orderDetailsCompleted = await _orderHeaderRepository.CheckOrderDetailsForOrderHeader(orderHeaderId);

            if (orderDetailsCompleted)
            {
                orderHeader.StatusName = OrderName.Realized;
                await _orderHeaderRepository.Update(orderHeaderId, orderHeader);

                return true;
            }

            return false;
        }
        catch (SmartWMSExceptionHandler e)
        {
            return false;
        }
    }
}