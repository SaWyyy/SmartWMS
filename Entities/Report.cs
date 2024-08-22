using System;
using System.Collections.Generic;
using SmartWMS.Models.Enums;

namespace SmartWMS.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public ReportType ReportType { get; set; }

    public ReportPeriod ReportPeriod { get; set; }

    public DateTime ReportDate { get; set; }

    public int WarehousesWarehouseId { get; set; }

    public byte[] ReportFile { get; set; } = null!;

    public virtual Warehouse WarehousesWarehouse { get; set; } = null!;
}
