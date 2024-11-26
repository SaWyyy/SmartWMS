using AutoMapper;
using SmartWMS.Entities.Enums;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services.Interfaces;

namespace SmartWMS.Services;

public class OrderCancellationService : IOrderCancellationService
{
    private readonly IOrderHeaderRepository _orderHeaderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IProductRepository _productRepository;
    private readonly ITaskRepository _taskRepository;

    public OrderCancellationService(
        IOrderHeaderRepository orderHeaderRepository, 
        IOrderDetailRepository orderDetailRepository,
        IProductRepository productRepository,
        ITaskRepository taskRepository)
    {
        this._orderHeaderRepository = orderHeaderRepository;
        this._orderDetailRepository = orderDetailRepository;
        this._productRepository = productRepository;
        this._taskRepository = taskRepository;
    }
    
    public async Task CancelOrder(int orderHeaderId)
    {
        try
        {
            var orderHeader = await _orderHeaderRepository.Get(orderHeaderId);
            if (orderHeader.StatusName == OrderName.Cancelled)
                throw new SmartWMSExceptionHandler("Order header is already cancelled");
            
            var orderDetails = await _orderDetailRepository.GetAllByOrderHeaderId(orderHeaderId);

            foreach (var orderDetail in orderDetails)
            {
                var product = await _productRepository.Get(orderDetail.ProductsProductId);
                product.Quantity += orderDetail.Quantity;
                await _productRepository.Update(orderDetail.ProductsProductId, product);

                orderDetail.Done = true;
                await _orderDetailRepository.Update(orderDetail.OrderDetailId.GetValueOrDefault(), orderDetail);
                
                var task = await _taskRepository.GetByOrderDetailId(orderDetail.OrderDetailId.GetValueOrDefault());
                task.Done = true;
                await _taskRepository.Update(task.TaskId.GetValueOrDefault(), task);
            }

            orderHeader.StatusName = OrderName.Cancelled;
            await _orderHeaderRepository.Update(orderHeaderId, orderHeader);
        }
        catch (SmartWMSExceptionHandler e)
        {
            throw new SmartWMSExceptionHandler(e.Message);
        }
    }
}