using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartWMS.Models;

namespace SmartWMS.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly SmartwmsDbContext _dbContext;
    private readonly IMapper _mapper;

    public CountryRepository(SmartwmsDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }

    public async Task<Country> Add(CountryDto dto)
    {
        var country = _mapper.Map<Country>(dto);
        await _dbContext.Countries.AddAsync(country);
        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
        {
            return country;
        }

        return null;
    }

    public async Task<IEnumerable<CountryDto>> GetAll()
    {
        var countries = await _dbContext.Countries.ToListAsync();
        var countryDto = _mapper.Map<List<CountryDto>>(countries);

        return countryDto;
    }

    public async Task<CountryDto> Get(int id)
    {
        var country = await _dbContext.Countries.FirstOrDefaultAsync(r => r.CountryId == id);

        if (country is null)
        {
            return null;
        }

        var result = _mapper.Map<CountryDto>(country);
        return result; 
    }

    public async Task<Country> Delete(int id)
    {
        var country = await _dbContext.Countries.FirstOrDefaultAsync(r => r.CountryId == id);

        if (country is null)
        {
            return null;
        }

        _dbContext.Countries.Remove(country);
        var result = await _dbContext.SaveChangesAsync();

        if (country is null)
        {
            return null;
        }

        return country;
    }

    public async Task<Country> Update(int id, CountryDto dto)
    {
        var country = await _dbContext.Countries.FirstOrDefaultAsync(r => r.CountryId == id);

        if (country is null)
        {
            return null;
        }

        country.CountryCode = dto.CountryCode;
        country.CountryName = dto.CountryName;

        var result = await _dbContext.SaveChangesAsync();

        if (result > 0)
        {
            return country;
        }

        return null;
    }
    
}