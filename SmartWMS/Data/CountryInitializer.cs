using Microsoft.EntityFrameworkCore;
using SmartWMS.Entities;
using Task = System.Threading.Tasks.Task;

namespace SmartWMS.Data;

public class CountryInitializer
{
    private readonly SmartwmsDbContext _dbContext;

    public CountryInitializer(SmartwmsDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task InitializeAsync()
    {
        var countries = new[]
        {
            new { Name = "Andora", Code = 376 },
            new { Name = "Austria", Code = 43 },
            new { Name = "Belgia", Code = 32 },
            new { Name = "Białoruś", Code = 375 },
            new { Name = "Bośnia i Hercegowina", Code = 387 },
            new { Name = "Bułgaria", Code = 359 },
            new { Name = "Chorwacja", Code = 385 },
            new { Name = "Czarnogóra", Code = 382 },
            new { Name = "Czechy", Code = 420 },
            new { Name = "Dania", Code = 45 },
            new { Name = "Estonia", Code = 372 },
            new { Name = "Finlandia", Code = 358 },
            new { Name = "Francja", Code = 33 },
            new { Name = "Grecja", Code = 30 },
            new { Name = "Hiszpania", Code = 34 },
            new { Name = "Holandia", Code = 31 },
            new { Name = "Irlandia", Code = 353 },
            new { Name = "Islandia", Code = 354 },
            new { Name = "Kosowo", Code = 383 },
            new { Name = "Liechtenstein", Code = 423 },
            new { Name = "Litwa", Code = 370 },
            new { Name = "Luksemburg", Code = 352 },
            new { Name = "Łotwa", Code = 371 },
            new { Name = "Macedonia Północna", Code = 389 },
            new { Name = "Malta", Code = 356 },
            new { Name = "Mołdawia", Code = 373 },
            new { Name = "Monako", Code = 377 },
            new { Name = "Niemcy", Code = 49 },
            new { Name = "Norwegia", Code = 47 },
            new { Name = "Polska", Code = 48 },
            new { Name = "Portugalia", Code = 351 },
            new { Name = "Rumunia", Code = 40 },
            new { Name = "San Marino", Code = 378 },
            new { Name = "Serbia", Code = 381 },
            new { Name = "Słowacja", Code = 421 },
            new { Name = "Słowenia", Code = 386 },
            new { Name = "Szwajcaria", Code = 41 },
            new { Name = "Szwecja", Code = 46 },
            new { Name = "Turcja", Code = 90 },
            new { Name = "Ukraina", Code = 380 },
            new { Name = "Watykan", Code = 379 },
            new { Name = "Węgry", Code = 36 },
            new { Name = "Wielka Brytania", Code = 44 },
            new { Name = "Włochy", Code = 39 }
        };
        
        var existingCountries = await _dbContext.Countries
            .Where(c => countries.Select(x => x.Name).Contains(c.CountryName))
            .Select(c => c.CountryName)
            .ToListAsync();

        var newCountries = countries
            .Where(c => !existingCountries.Contains(c.Name))
            .Select(c => new Country { CountryName = c.Name, CountryCode = c.Code })
            .ToList();
        
        if (newCountries.Any())
        {
            await _dbContext.Countries.AddRangeAsync(newCountries);
            await _dbContext.SaveChangesAsync();
        }
    }
}