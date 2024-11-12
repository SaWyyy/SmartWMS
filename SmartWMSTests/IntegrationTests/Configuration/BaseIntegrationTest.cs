using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SmartWMS;

namespace SmartWMSTests.IntegrationTests.Configuration;

public abstract class BaseIntegrationTest
{
    private readonly IServiceScope _scope;
    protected JsonSerializerOptions customJsonOptions { get; }
    protected MapperConfiguration _mapperConfiguration { get; }
    
    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();

        customJsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        _mapperConfiguration = new MapperConfiguration(config =>
        {
            config.AddProfile<MappingProfiles>();
        });
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
    
}