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

            services.AddDbContext<SmartwmsDbContext>(options =>
            {
                options.UseNpgsql(ConfigureDb(_dbContainer.GetConnectionString()));
            });
            
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
                dbContext.Database.EnsureCreated();
            }
            
            InitializeRolesAsync(services).GetAwaiter().GetResult();
            InitializeWarehouseAsync(services).GetAwaiter().GetResult();
            // InitializeAdminUserAsync(services).GetAwaiter().GetResult();
        });
    }
    
    private async Task MigrateDatabaseAsync(IServiceCollection services)
    {
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
    
    private async Task InitializeRolesAsync(IServiceCollection services)
    {
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Manager", "Employee" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }

    private async Task InitializeWarehouseAsync(IServiceCollection services)
    {
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();
            var warehouse = await dbContext.Warehouses.FirstOrDefaultAsync(x => x.WarehouseId == 1);
            if (warehouse is null)
            {
                string address = "Nadbystrzycka 38 a";
                var newWarehouse = new Warehouse
                {
                    Address = address
                };

                await dbContext.AddAsync(newWarehouse);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private async Task InitializeAdminUserAsync(IServiceCollection services)
    {
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<SmartwmsDbContext>();

            string email = "admin@admin.com";
            string userName = "Admin";
            string password = "Admin123@";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new User()
                {
                    Email = email,
                    UserName = userName,
                    PasswordHash = password,
                    WarehousesWarehouseId = dbContext.Warehouses.FirstOrDefaultAsync(x => x.WarehouseId == 1).Id
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
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