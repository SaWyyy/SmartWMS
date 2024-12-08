using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services.Interfaces;

namespace SmartWMS.Services;

public class BarcodeGeneratorService : IBarcodeGeneratorService
{
    private readonly IProductRepository _productRepository;

    public BarcodeGeneratorService(IProductRepository productRepository)
    {
        this._productRepository = productRepository;
    }
    
    public async Task<string> GenerateBarcode()
    {
        var productsDtos = await _productRepository.GetAll();
        var products = productsDtos.ToList();

        var random = new Random();
        string ean13Code;
        do
        {
            string baseDigits = new string(Enumerable.Range(0, 12).Select(_ => random.Next(0, 10).ToString()[0]).ToArray());
            int checkDigit = CalculateEan13CheckDigit(baseDigits);
            ean13Code = baseDigits + checkDigit;

        } while (products.Any(product => product.Barcode == ean13Code));

        return ean13Code;
    }
    
    private int CalculateEan13CheckDigit(string baseDigits)
    {
        if (baseDigits.Length != 12 || !baseDigits.All(char.IsDigit))
            throw new ArgumentException("Podstawa kodu EAN-13 musi mieć dokładnie 12 cyfr.");

        int sum = 0;

        for (int i = 0; i < baseDigits.Length; i++)
        {
            int digit = int.Parse(baseDigits[i].ToString());
            sum += (i % 2 == 0) ? digit : digit * 3;
        }

        int checkDigit = (10 - (sum % 10)) % 10;

        return checkDigit;
    }
}