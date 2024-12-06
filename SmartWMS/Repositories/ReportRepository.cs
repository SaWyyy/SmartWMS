using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMS.Repositories;

public class ReportRepository: IReportRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public ReportRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }

    public async Task<byte[]> AttachFile(IFormFile file, int id)
    {
        var report = await _dbContext.Reports.FirstOrDefaultAsync(r => r.ReportId == id);

        if (report is null)
            throw new SmartWMSExceptionHandler("Wrong report's id has been provided");
        
        if (file is null || file.Length == 0)
            throw new SmartWMSExceptionHandler("File does not exist or does not have any content");
        
        byte[] fileData;

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            fileData = memoryStream.ToArray();
        }

        report.ReportFile = fileData;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return fileData;

        throw new SmartWMSExceptionHandler("Error occurred while trying to save file");
    }

    public async Task<ReportDto> Add(ReportDto dto)
    {
        
        var warehouse = await _dbContext.Warehouses.FirstOrDefaultAsync(w => w.WarehouseId == 1);

        if (warehouse is null)
            throw new SmartWMSExceptionHandler("Warehouse with specified id hasn't been found");

        byte[] fileData;


        var report = new Report
        {
            ReportType = dto.ReportType,
            ReportPeriod = dto.ReportPeriod,
            ReportDate = dto.ReportDate,
            WarehousesWarehouseId = 1,
            ReportFile = new byte[]{0x0}
        };

        await _dbContext.Reports.AddAsync(report);

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return _mapper.Map<ReportDto>(report);

        throw new SmartWMSExceptionHandler("Error has occured while adding report");
    }

    public async Task<IEnumerable<ReportDto>> GetAll()
    {
        var reports = await _dbContext.Reports.ToListAsync();
        return _mapper.Map<IEnumerable<ReportDto>>(reports);
    }

    public async Task<ReportDto> Get(int id)
    {
        var report = await _dbContext.Reports.FirstOrDefaultAsync(r => r.ReportId == id);

        if (report is null)
            throw new SmartWMSExceptionHandler("Report with specified id hasn't been found");

        return _mapper.Map<ReportDto>(report);
    }

    public async Task<Report> Delete(int id)
    {
        var report = await _dbContext.Reports.FirstOrDefaultAsync(r => r.ReportId == id);
        
        if(report is null)
            throw new SmartWMSExceptionHandler("Report with specified id hasn't been found");

        _dbContext.Remove(report);

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return report;

        throw new SmartWMSExceptionHandler("Error has occured while saving changes to report table");
    }

    public async Task<Report> Update(int id, ReportDto dto)
    {
        var report = await _dbContext.Reports.FirstOrDefaultAsync(r => r.ReportId == id);

        if (report is null) 
            throw new SmartWMSExceptionHandler("Report with specified id hasn't been found");

        report.ReportType = dto.ReportType;
        report.ReportPeriod = dto.ReportPeriod;
        report.ReportDate = dto.ReportDate;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
            return report;
        
        throw new SmartWMSExceptionHandler("Error has occured while saving changes to report table");
    }

    public async Task<byte[]> DownloadFile(int id)
    {
        var report = await _dbContext.Reports.FirstOrDefaultAsync(r => r.ReportId == id);
        
        if(report is null)
            throw new SmartWMSExceptionHandler("Cannot download requested file: report with specified id hasn't been found");

        var file = report.ReportFile;
        return file;
    }
}