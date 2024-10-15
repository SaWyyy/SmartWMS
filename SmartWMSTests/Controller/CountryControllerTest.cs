using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using SmartWMS;
using SmartWMS.Controllers;
using SmartWMS.Entities;
using SmartWMS.Models.DTOs;
using SmartWMS.Repositories.Interfaces;

namespace SmartWMSTests.Controller;

public class CountryControllerTest
{
    private readonly ICountryRepository _countryRepository;
    private readonly CountryController _countryController;
    private readonly ILogger<CountryController> _logger;

    public CountryControllerTest()
    {
        this._countryRepository = A.Fake<ICountryRepository>();
        this._logger = A.Fake<ILogger<CountryController>>();
        this._countryController = new CountryController(_countryRepository, _logger);
    }

    private static Country CreateFakeCountry()
    {
        var country = A.Fake<Country>();
        country.CountryId = 1;
        country.CountryName = "Test name";
        country.CountryCode = 123;
        country.Waybills = new List<Waybill>();
        return country;
    }

    private static CountryDto CreateFakeCountryDto()
    {
        var countryDto = A.Fake<CountryDto>();
        countryDto.CountryId = 1;
        countryDto.CountryName = "Test name";
        countryDto.CountryCode = 123;
        return countryDto;
    }

    [Fact]
    public async void CountryController_Add_ReturnsOk()
    {
        // Arrange
        var country = CreateFakeCountry();
        var countryDto = CreateFakeCountryDto();
        
        // Act
        A.CallTo(() => _countryRepository.Add(countryDto)).Returns(country);
        var result = (OkObjectResult)await _countryController.AddCountry(countryDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void CountryController_Add_ReturnsBadRequest()
    {
        // Arrange
        var countryDto = CreateFakeCountryDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _countryRepository.Add(countryDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _countryController.AddCountry(countryDto);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }

    [Fact]
    public async void CountryController_GetAll_ReturnsOk()
    {
        // Arrange
        var countries = new List<CountryDto>();
        countries.Add(CreateFakeCountryDto());
        
        // Act
        A.CallTo(() => _countryRepository.GetAll()).Returns(countries);
        var result = (OkObjectResult)await _countryController.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CountryController_Get_ReturnsOk(int id)
    {
        // Arrange
        var countryDto = CreateFakeCountryDto();
        countryDto.CountryId = id;
        
        // Act
        A.CallTo(() => _countryRepository.Get(id)).Returns(countryDto);
        var result = (OkObjectResult)await _countryController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CountryController_Get_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An exception occured";
        
        // Act
        A.CallTo(() => _countryRepository.Get(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _countryController.Get(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CountryController_Delete_ReturnsOk(int id)
    {
        // Arrange
        var country = CreateFakeCountry();
        country.CountryId = id;
        
        // Act
        A.CallTo(() => _countryRepository.Delete(id)).Returns(country);
        var result = (OkObjectResult)await _countryController.Delete(id);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CountryController_Delete_ReturnsNotFound(int id)
    {
        // Arrange
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _countryRepository.Delete(id))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (NotFoundObjectResult)await _countryController.Delete(id);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CountryController_Update_ReturnsOk(int id)
    {
        // Arrange
        var countryDto = CreateFakeCountryDto();
        var country = CreateFakeCountry();
        country.CountryId = id;

        // Act
        A.CallTo(() => _countryRepository.Update(id, countryDto)).Returns(country);
        var result = (OkObjectResult)await _countryController.Update(id, countryDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    public async void CountryController_Update_ReturnsBadRequest(int id)
    {
        // Arrange
        var countryDto = CreateFakeCountryDto();
        const string exceptionMessage = "An error occured";
        
        // Act
        A.CallTo(() => _countryRepository.Update(id, countryDto))
            .Throws(new SmartWMSExceptionHandler(exceptionMessage));
        var result = (BadRequestObjectResult)await _countryController.Update(id, countryDto);
        
        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        result.Should().NotBeNull();
    }
}