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
    private readonly IShelfRepository _shelfRepository;
    private readonly IWaybillRepository _waybillRepository;

    public OrderCancellationService(
        IOrderHeaderRepository orderHeaderRepository, 
        IOrderDetailRepository orderDetailRepository,
        IProductRepository productRepository,
        ITaskRepository taskRepository,
        IShelfRepository shelfRepository,
        IWaybillRepository waybillRepository)
    {
        this._orderHeaderRepository = orderHeaderRepository;
        this._orderDetailRepository = orderDetailRepository;
        this._productRepository = productRepository;
        this._taskRepository = taskRepository;
        this._shelfRepository = shelfRepository;
        this._waybillRepository = waybillRepository;
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

                await RetrieveShelfAllocation(product.ProductId.GetValueOrDefault());
                
                orderDetail.Done = true;
                await _orderDetailRepository.Update(orderDetail.OrderDetailId.GetValueOrDefault(), orderDetail);
                
                var task = await _taskRepository.GetByOrderDetailId(orderDetail.OrderDetailId.GetValueOrDefault());
                task.Done = true;
                await _taskRepository.Update(task.TaskId.GetValueOrDefault(), task);
            }

            orderHeader.StatusName = OrderName.Cancelled;
            await _orderHeaderRepository.Update(orderHeaderId, orderHeader);

            var waybill = await _waybillRepository.GetByOrderHeaderId(orderHeaderId);
            await _waybillRepository.Delete(waybill.WaybillId.GetValueOrDefault());
        }
        catch (SmartWMSExceptionHandler e)
        {
            throw new SmartWMSExceptionHandler(e.Message);
        }
    }

    private async Task RetrieveShelfAllocation(int productId)
    {
        var allocations = await _shelfRepository.GetAllocationsByProduct(productId);
        foreach (var allocation in allocations)
        {
            var shelf = await _shelfRepository.Get(allocation.ShelfId);
            shelf.CurrentQuant += allocation.Quantity;
            await _shelfRepository.Update(shelf.ShelfId.GetValueOrDefault(), shelf);
        }

        await _shelfRepository.DeleteAllocationsByProduct(productId);
    }
}