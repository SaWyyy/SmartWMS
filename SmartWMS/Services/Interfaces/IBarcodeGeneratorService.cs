namespace SmartWMS.Services.Interfaces;

public interface IBarcodeGeneratorService
{
    Task<string> GenerateBarcode();
}