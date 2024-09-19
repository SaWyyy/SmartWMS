using SmartWMS.Entities.Enums;

namespace SmartWMS.Models.DTOs;

public class ReportDto
{
    public int? ReportId { get; set; }
    public ReportType ReportType { get; set; }

    public ReportPeriod ReportPeriod { get; set; }

    public DateTime ReportDate { get; set; }
}