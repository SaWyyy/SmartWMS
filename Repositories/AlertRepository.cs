using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public AlertRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }
    
    public async Task<Alert> Add(AlertDto dto)
    {
        var warehouse = await _dbContext.Warehouses.FirstOrDefaultAsync(x => x.WarehouseId == 1);

        if (warehouse is null)
            throw new SmartWMSExceptionHandler("Warehouse hasn't been found");

        var alert = new Alert
        {
            Seen = false,
            AlertDate = dto.AlertDate,
            AlertType = dto.AlertType,
            WarehousesWarehouseId = 1
        };

        await _dbContext.Alerts.AddAsync(alert);

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return alert;

        throw new SmartWMSExceptionHandler("Error has occured while adding alert");
    }

    public async Task<IEnumerable<AlertDto>> GetAll()
    {
        var result = await _dbContext.Alerts.ToListAsync();

        return _mapper.Map<List<AlertDto>>(result);
    }

    public async Task<AlertDto> Get(int id)
    {
        var result = await _dbContext.Alerts.FirstOrDefaultAsync(x => x.AlertId == id);

        if (result is null)
            throw new SmartWMSExceptionHandler("Alert hasn't been found");

        return _mapper.Map<AlertDto>(result);
    }

    public async Task<Alert> Update(int id, AlertDto dto)
    {
        var result = await _dbContext.Alerts.FirstOrDefaultAsync(x => x.AlertId == id);

        if (result is null)
            throw new SmartWMSExceptionHandler("Alert hasn't been found");

        result.AlertDate = dto.AlertDate;
        result.AlertType = dto.AlertType;
        result.Seen = dto.Seen;

        var result2 = await _dbContext.SaveChangesAsync();

        if (result2 > 0)
            return result;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to alerts table");
    }

    public async Task<Alert> Delete(int id)
    {
        var result = await _dbContext.Alerts.FirstOrDefaultAsync(x => x.AlertId == id);

        if (result is null)
            throw new SmartWMSExceptionHandler("Alert hasn't been found");

        _dbContext.Alerts.Remove(result);

        var result2 = await _dbContext.SaveChangesAsync();

        if (result2 > 0)
            return result;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to alerts table");
    }

    public async Task<Alert> ChangeSeen(int id)
    {
        var result = await _dbContext.Alerts.FirstOrDefaultAsync(x => x.AlertId == id);

        if (result is null)
            throw new SmartWMSExceptionHandler("Alert hasn't been found");

        result.Seen = true;

        var result2 = await _dbContext.SaveChangesAsync();

        if (result2 > 0)
            return result;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to alerts table");
    }
}