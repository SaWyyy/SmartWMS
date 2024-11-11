using Microsoft.Extensions.DependencyInjection;

namespace SmartWMSTests.IntegrationTests.Configuration;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
    }
}