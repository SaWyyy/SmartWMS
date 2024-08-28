using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Models;

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
    
    public async Task<OrderHeader?> Add(OrderHeaderDto dto)
    {
        var waybill = await _dbContext.Waybills.FirstOrDefaultAsync(r => r.WaybillId == dto.WaybillsWaybillId);

        if (waybill is null)
            return null;
        
        var order = _mapper.Map<OrderHeader>(dto);
        await _dbContext.OrderHeaders.AddAsync(order);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return order;

        return null;
    }

    public async Task<IEnumerable<OrderHeaderDto>> GetAll()
    {
        var orders = await _dbContext.OrderHeaders.ToListAsync();

        var ordersDto = _mapper.Map<List<OrderHeaderDto>>(orders);

        return ordersDto;
    }

    public async Task<OrderHeaderDto?> Get(int id)
    {
        var order = await _dbContext.OrderHeaders.FirstOrDefaultAsync(r => r.OrdersHeaderId == id);

        if (order is null)
            return null;

        var orderDto = _mapper.Map<OrderHeaderDto>(order);

        return orderDto;
    }

    public async Task<OrderHeader?> Delete(int id)
    {
        var order = await _dbContext.OrderHeaders.FirstOrDefaultAsync(r => r.OrdersHeaderId == id);

        if (order is null)
            return null;
        
        _dbContext.OrderHeaders.Remove(order);

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return order;

        return null;
    }

    public async Task<OrderHeader?> Update(int id, OrderHeaderDto dto)
    {
        var order = await _dbContext.OrderHeaders.FirstOrDefaultAsync(r => r.OrdersHeaderId == id);

        if (order is null)
            return null;

        order.OrderDate = dto.OrderDate;
        order.DeliveryDate = dto.DeliveryDate;
        order.DestinationAddress = dto.DestinationAddress;
        order.WaybillsWaybillId = dto.WaybillsWaybillId;
        order.TypeName = dto.TypeName;
        order.StatusName = dto.StatusName;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return order;

        return null;
    }
}