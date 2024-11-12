using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            services.RemoveAll(typeof(DbContextOptions<SmartwmsDbContext>));
            
            var dbOptions = new DbContextOptionsBuilder<SmartwmsDbContext>()
                .UseNpgsql(ConfigureDb(_dbContainer.GetConnectionString()))
                .Options;

            services.AddSingleton(dbOptions);
            services.AddDbContext<SmartwmsDbContext>(options =>
            {
                options.UseNpgsql(ConfigureDb(_dbContainer.GetConnectionString()));
            });
            
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
                dbContext.Database.EnsureCreated();
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

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        Environment.SetEnvironmentVariable("ConnectionStrings:DefaultConnection", _dbContainer.GetConnectionString());
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}