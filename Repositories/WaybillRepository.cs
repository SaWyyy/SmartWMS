using System.Security;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Repositories;

public class WaybillRepository : IWaybillRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public WaybillRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }

    public async Task<Waybill> AddWaybill(WaybillDto dto)
    {
        var country = await _dbContext.Countries.FirstOrDefaultAsync(r => r.CountryId == dto.CountriesCountryId);

        if (country is null)
            throw new SmartWMSExceptionHandler("Country with specified id hasn't been found");

        var orderHeader =
            await _dbContext.OrderHeaders.FirstOrDefaultAsync(r => r.OrdersHeaderId == dto.OrderHeadersOrderHeaderId);

        if (orderHeader is null)
            throw new SmartWMSExceptionHandler("Order header with specified id hasn't been found while trying to create waybill record");

        var duplicated_waybill =
            await _dbContext.Waybills.FirstOrDefaultAsync(r =>
                r.OrderHeadersOrderHeaderId == dto.OrderHeadersOrderHeaderId);

        if (duplicated_waybill is not null)
            throw new SmartWMSExceptionHandler("Cannot assign order header's id to more than one waybill");
        
        var waybill = _mapper.Map<Waybill>(dto);
        await _dbContext.Waybills.AddAsync(waybill);
        var result = await _dbContext.SaveChangesAsync();
        
        if (result > 0)
            return waybill;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to waybill table");
    }

    public async Task<IEnumerable<WaybillDto>> GetAll()
    {
        var waybills = await _dbContext.Waybills.ToListAsync();
        var waybillsDto = _mapper.Map<List<WaybillDto>>(waybills);
        return waybillsDto;
    }

    public async Task<WaybillDto> Get(int id)
    {
        var waybill = await _dbContext.Waybills.FirstOrDefaultAsync(r => r.WaybillId == id);

        if (waybill is null)
            throw new SmartWMSExceptionHandler("Waybill with specified id hasn't been found");

        var result = _mapper.Map<WaybillDto>(waybill);
        return result;
    }

    public async Task<Waybill> Delete(int id)
    {
        var waybill = await _dbContext.Waybills.FirstOrDefaultAsync(r => r.WaybillId == id);

        if (waybill is null)
            throw new SmartWMSExceptionHandler("Waybill with specified id hasn't been found");
        
        var orderHeader = await _dbContext.OrderHeaders.FirstOrDefaultAsync(r => r.OrdersHeaderId == waybill.OrderHeadersOrderHeaderId);
        
        if (orderHeader is null)
            throw new SmartWMSExceptionHandler("Internal error: waybill cannot exist without order header");

        _dbContext.Waybills.Remove(waybill);
        
        var result2 = await _dbContext.SaveChangesAsync();

        if (result2 > 0)
            return waybill;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to waybill table");
    }

    public async Task<Waybill> Update(int id, WaybillDto dto)
    {
        var waybill = await _dbContext.Waybills.FirstOrDefaultAsync(r => r.WaybillId == id);

        if (waybill is null)
            throw new SmartWMSExceptionHandler("Waybill with specified id hasn't been found");

        var countryId = await _dbContext.Countries.FirstOrDefaultAsync(r => r.CountryId == dto.CountriesCountryId);

        if (countryId is null)
            throw new SmartWMSExceptionHandler("Cannot assign new CountryId because provided one doesn't exist");

        waybill.CountriesCountryId = dto.CountriesCountryId;
        waybill.LoadingDate = dto.LoadingDate;
        waybill.SupplierName = dto.SupplierName;
        waybill.ShippingDate = dto.ShippingDate;
        waybill.PostalCode = dto.PostalCode;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return waybill;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to waybill table");
    }
    
}