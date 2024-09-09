using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Repositories;

public class OrderHeaderRepository : IOrderHeaderRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public OrderHeaderRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }
    
    public async Task<OrderHeader> Add(OrderHeaderDto dto)
    {
        var order = _mapper.Map<OrderHeader>(dto);
        await _dbContext.OrderHeaders.AddAsync(order);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return order;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes");
    }

    public async Task<IEnumerable<OrderHeaderDto>> GetAll()
    {
        var orders = await _dbContext.OrderHeaders.ToListAsync();

        var ordersDto = _mapper.Map<List<OrderHeaderDto>>(orders);

        return ordersDto;
    }

    public async Task<OrderHeaderDto> Get(int id)
    {
        var order = await _dbContext.OrderHeaders.FirstOrDefaultAsync(r => r.OrdersHeaderId == id);

        if (order is null)
            throw new SmartWMSExceptionHandler("OrderHeader with specified id hasn't been found");

        var orderDto = _mapper.Map<OrderHeaderDto>(order);

        return orderDto;
    }

    public async Task<OrderHeader> Delete(int id)
    {
        var waybill = await _dbContext.Waybills.FirstOrDefaultAsync(r => r.OrderHeadersOrderHeaderId == id);

        if (waybill is not null)
        {
            _dbContext.Waybills.Remove(waybill);
            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
                throw new SmartWMSExceptionHandler("Error has occured while saving changes to waybill table");
        }
        
        var order = await _dbContext.OrderHeaders.FirstOrDefaultAsync(r => r.OrdersHeaderId == id);

        if (order is null)
            throw new SmartWMSExceptionHandler("OrderHeader with specified id hasn't been found");
        
        _dbContext.OrderHeaders.Remove(order);

        var result2 = await _dbContext.SaveChangesAsync();

        if (result2 > 0)
            return order;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to order header table");
    }

    public async Task<OrderHeader> Update(int id, OrderHeaderDto dto)
    {
        var order = await _dbContext.OrderHeaders.FirstOrDefaultAsync(r => r.OrdersHeaderId == id);

        if (order is null)
            throw new SmartWMSExceptionHandler("OrderHeader with specified id hasn't been found");

        order.OrderDate = dto.OrderDate;
        order.DeliveryDate = dto.DeliveryDate;
        order.DestinationAddress = dto.DestinationAddress;
        order.TypeName = dto.TypeName;
        order.StatusName = dto.StatusName;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return order;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to order header table");
    }
}