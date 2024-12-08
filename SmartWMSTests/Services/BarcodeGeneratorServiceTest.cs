using FakeItEasy;
using FluentAssertions;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services;
using SmartWMS.Services.Interfaces;

namespace SmartWMSTests.Services;

public class BarcodeGeneratorServiceTest
{
    private readonly IProductRepository _productRepository;
    private readonly IBarcodeGeneratorService _service;

    public BarcodeGeneratorServiceTest()
    {
        this._productRepository = A.Fake<IProductRepository>();
        this._service = new BarcodeGeneratorService(_productRepository);
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

    [Fact]
    public async Task GenerateBarcode_ShouldReturnString()
    {
        // Arrange
        A.CallTo(() => _productRepository.GetAll()).Returns(Enumerable.Empty<ProductDto>().ToList());
        
        // Act
        var result = await _service.GenerateBarcode();
        var codeBase = result.Substring(0, 12);
        var checkDigit = int.Parse(result[result.Length - 1].ToString());

        // Assert
        result.Length.Should().Be(13);
        checkDigit.Should().Be(CalculateEan13CheckDigit(codeBase));
    }
}