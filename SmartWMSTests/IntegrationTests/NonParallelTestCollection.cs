using SmartWMSTests.IntegrationTests.Configuration;

namespace SmartWMSTests.IntegrationTests;

[CollectionDefinition("Non-Parallel tests", DisableParallelization = true)]
public class NonParallelTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>
{
    
}