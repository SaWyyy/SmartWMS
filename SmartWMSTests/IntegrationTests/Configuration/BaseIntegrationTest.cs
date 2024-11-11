using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace SmartWMSTests.IntegrationTests.Configuration;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected JsonSerializerOptions customJsonOptions { get; }
    
    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();

        customJsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
    
}