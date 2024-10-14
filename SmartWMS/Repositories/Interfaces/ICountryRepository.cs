using SmartWMS.Entities;
using SmartWMS.Models.DTOs;

namespace SmartWMS.Repositories.Interfaces;

public interface ICountryRepository
{
    Task<Country> Add(CountryDto dto);
    Task<IEnumerable<CountryDto>> GetAll();
    Task<CountryDto> Get(int id);
    Task<Country> Delete(int id);
    Task<Country> Update(int id, CountryDto dto);
}