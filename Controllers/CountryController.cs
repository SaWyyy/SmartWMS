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

    [HttpPost("")]
    public async Task<IActionResult> AddCountry(CountryDto dto)
    {
        var result = await _countryRepository.Add(dto);
        
        if (result is null)
        {
            _logger.LogError("Error has occured while adding country");
            return BadRequest("Error has occured while adding country");
        }
        
        _logger.LogInformation($"Country {dto.CountryCode} has been added");
        return Ok($"Adding country completed, Id: {result.CountryId}");
        
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var result = await _countryRepository.GetAll();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _countryRepository.Get(id);

        if (result is null)
        {
            _logger.LogError("Error has occured while looking for country");
            return NotFound("Error has occured while looking for shelf");
        }
        
        _logger.LogInformation("Country found");
        return Ok(result);
        
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedCountry = await _countryRepository.Delete(id);

        if (deletedCountry is null)
        {
            _logger.LogError("Country with specified ID hasn't been found");
            return NotFound("Country with specified ID hasn't been found");
        }
        
        _logger.LogInformation("Shelf removed");
        return Ok(deletedCountry);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CountryDto dto)
    {
        var updatedCountry = await _countryRepository.Update(id, dto);
        if (updatedCountry is null)
        {
            _logger.LogError("Country with specified ID hasn't been edited");
            return BadRequest("Error has occured while editing country");
        }
        
        _logger.LogInformation("Country edited");
        return Ok(updatedCountry);
    }
    
    
}