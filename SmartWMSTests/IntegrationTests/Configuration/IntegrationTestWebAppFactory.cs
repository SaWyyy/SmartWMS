using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using Testcontainers.PostgreSql;
using Task = System.Threading.Tasks.Task;

namespace SmartWMSTests.IntegrationTests.Configuration;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("smartwms_db")
        .WithUsername("postgres")
        .WithPassword("root")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes("Test")
                    .RequireAuthenticatedUser()
                    .Build();
            });
            
            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<SmartwmsDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<SmartwmsDbContext>(options =>
            {
                options.UseNpgsql(ConfigureDb(_dbContainer.GetConnectionString()));
            });

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
                dbContext.Database.Migrate();
            }
        });
    }

    private NpgsqlDataSource ConfigureDb(string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        
        dataSourceBuilder.MapEnum<ActionType>("action_type");
        dataSourceBuilder.MapEnum<AlertType>("alert_type");
        dataSourceBuilder.MapEnum<LevelType>("level_type");
        dataSourceBuilder.MapEnum<OrderName>("order_name");
        dataSourceBuilder.MapEnum<OrderType>("order_type");
        dataSourceBuilder.MapEnum<ReportType>("report_type");
        dataSourceBuilder.MapEnum<ReportPeriod>("report_period");

        var dataSource = dataSourceBuilder.Build();
        return dataSource;
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}