using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartWMS.Models;
using SmartWMS.Repositories;

namespace SmartWMS.Controllers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly ICountryRepository _countryRepository;
    private readonly ILogger<CountryController> _logger;

    public CountryController(ICountryRepository countryRepository, ILogger<CountryController> logger)
    {
        this._countryRepository = countryRepository;
        this._logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddCountry(CountryDto dto)
    {
        try
        {
            var result = await _countryRepository.Add(dto);
            
            _logger.LogInformation($"Country {result.CountryCode} has been added");
            return Ok($"Adding country completed, Id: {result.CountryId}");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _countryRepository.GetAll();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var result = await _countryRepository.Get(id);
            
            _logger.LogInformation("Country found");
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deletedCountry = await _countryRepository.Delete(id);

            _logger.LogInformation("Shelf removed");
            return Ok(deletedCountry);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CountryDto dto)
    {
        try
        {
            var updatedCountry = await _countryRepository.Update(id, dto);

            _logger.LogInformation("Country edited");
            return Ok(updatedCountry);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}