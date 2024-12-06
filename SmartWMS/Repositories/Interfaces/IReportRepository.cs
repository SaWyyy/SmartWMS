using Microsoft.AspNetCore.Mvc;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface IReportRepository
{
    Task<ReportDto> Add(ReportDto dto);
    Task<IEnumerable<ReportDto>> GetAll();
    Task<ReportDto> Get(int id);
    Task<Report> Delete(int id);
    Task<Report> Update(int id, ReportDto dto);
    Task<byte[]> AttachFile(IFormFile file, int id);

    Task<byte[]> DownloadFile(int id);
}