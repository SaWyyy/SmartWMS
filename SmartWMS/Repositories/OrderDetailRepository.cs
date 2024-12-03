using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Repositories;

public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public OrderDetailRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }
    
    public async Task<OrderDetail> Add(OrderDetailDto dto)
    {
        dto.OrderDetailId = null;
        dto.Done = false;
        var orderHeader =
            await _dbContext.OrderHeaders.FirstOrDefaultAsync(x => 
                x.OrdersHeaderId == dto.OrderHeadersOrdersHeaderId);

        if (orderHeader is null)
            throw new SmartWMSExceptionHandler("Order header hasn't been found");

        var product = 
            await _dbContext.Products.FirstOrDefaultAsync(x => 
                x.ProductId == dto.ProductsProductId);

        if (product is null)
            throw new SmartWMSExceptionHandler("Product hasn't been found");

        if (product.Quantity < dto.Quantity)
            throw new SmartWMSExceptionHandler(
                "Not sufficient amount of product in the warehouse. Cannot create order detail");

        var orderDetail = _mapper.Map<OrderDetail>(dto);

        await _dbContext.OrderDetails.AddAsync(orderDetail);

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return orderDetail;

        throw new SmartWMSExceptionHandler("Error has occured while adding order detail");

    }

    public async Task<IEnumerable<OrderDetailDto>> GetAll()
    {
        var result = await _dbContext.OrderDetails.ToListAsync();

        return _mapper.Map<List<OrderDetailDto>>(result);
    }

    public async Task<IEnumerable<OrderDetailDto>> GetAllByOrderHeaderId(int orderHeaderId)
    {
        var orderHeader = await _dbContext.OrderHeaders
            .FirstOrDefaultAsync(x => x.OrdersHeaderId == orderHeaderId);

        if (orderHeader is null)
            throw new SmartWMSExceptionHandler("Order header does not exist");
        
        var result = await _dbContext.OrderDetails
            .IgnoreQueryFilters()
            .Where(x => x.OrderHeadersOrdersHeaderId == orderHeaderId)
            .ToListAsync();

        return _mapper.Map<List<OrderDetailDto>>(result);
    }


    public async Task<OrderDetailDto> Get(int id)
    {
        var result = 
            await _dbContext.OrderDetails.FirstOrDefaultAsync(x => x.OrderDetailId == id);

        if (result is null)
            throw new SmartWMSExceptionHandler("Order detail hasn't been found");

        return _mapper.Map<OrderDetailDto>(result);
    }

    public async Task<OrderDetail> Update(int id, OrderDetailDto dto)
    {
        var orderDetail = 
            await _dbContext.OrderDetails.FirstOrDefaultAsync(x => x.OrderDetailId == id);

        if (orderDetail is null)
            throw new SmartWMSExceptionHandler("Order detail hasn't been found");

        orderDetail.Quantity = dto.Quantity;
        orderDetail.Done = dto.Done;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return orderDetail;

        throw new SmartWMSExceptionHandler("Error has occured while updating order detail");
    }

    public async Task<OrderDetail> Delete(int id)
    {
        var orderDetail = 
            await _dbContext.OrderDetails.FirstOrDefaultAsync(x => x.OrderDetailId == id);

        if (orderDetail is null)
            throw new SmartWMSExceptionHandler("Order detail hasn't been found");

        _dbContext.OrderDetails.Remove(orderDetail);

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return orderDetail;

        throw new SmartWMSExceptionHandler("Error has occured while adding order detail");
    }

    public async Task<bool> CheckTasksForOrderDetail(int id)
    {
        var orderDetail = await _dbContext.OrderDetails
            .IgnoreQueryFilters()
            .Include(x => x.TasksTask)
            .FirstOrDefaultAsync(x => x.OrderDetailId == id);

        if (orderDetail is null)
            return false;

        if (orderDetail.TasksTask is not null && orderDetail.TasksTask!.Done)
        {
            orderDetail.TasksTask.FinishDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}