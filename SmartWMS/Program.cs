using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;
using NLog;
using Npgsql;
using SmartWMS.Data;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;
using SmartWMS.Services;
using SmartWMS.Services.Interfaces;
using SmartWMS.SignalR;
using Swashbuckle.AspNetCore.Filters;
using Task = System.Threading.Tasks.Task;

//====================================================================================================//
 //Configure logging for the application and init log//
//==================================================//
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
//===================================================================================================//



  //====================================================================================================//
 //Try-catch block to handle application errors//
//============================================//

try
{
    var builder = WebApplication.CreateBuilder(args);

    
      //====================================================================================================//
     //Add services to the dependency injection container//
    //==================================================//
    
    builder.Services.AddControllers();
    builder.Services.AddSignalR() // Adds SignalR
        .AddJsonProtocol(options =>
        {
            options.PayloadSerializerOptions.PropertyNamingPolicy = null;
        });
    builder.Services.AddEndpointsApiExplorer(); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme //Adds security definition
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        options.OperationFilter<SecurityRequirementsOperationFilter>(); //Adds an operation filter to Swagger
    });
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //Adds support for AutoMapper

    builder.Services.AddHangfire(x =>
        x.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddHangfireServer();
    //===================================================================================================//


    builder.Logging.ClearProviders(); //Remove default logging providers
    builder.Host.UseNLog(); //Sets Nlog as the logging provider

      //====================================================================================================//
     //Register repositories for Dependency Injection//
    //===============================================//
    builder.Services.AddTransient<IUserRepository, UserRepository>();
    builder.Services.AddTransient<IShelfRepository, ShelfRepository>();
    builder.Services.AddTransient<ICountryRepository, CountryRepository>();
    builder.Services.AddTransient<IWaybillRepository, WaybillRepository>();
    builder.Services.AddTransient<ITaskRepository, TaskRepository>();
    builder.Services.AddTransient<IOrderHeaderRepository, OrderHeaderRepository>();
    builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
    builder.Services.AddTransient<ISubcategoryRepository, SubcategoryRepository>();
    builder.Services.AddTransient<IAlertRepository, AlertRepository>();
    builder.Services.AddTransient<IProductRepository, ProductRepository>();
    builder.Services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
    builder.Services.AddTransient<IReportRepository, ReportRepository>();
    builder.Services.AddTransient<ILaneRepository, LaneRepository>();
    builder.Services.AddTransient<IRackRepository, RackRepository>();
    //====================================================================================================//
    
    
    //====================================================================================================//
    //Register services for Dependency Injection//
    //===============================================//
    builder.Services.AddScoped<IOrderValidationService, OrderValidationService>();
    builder.Services.AddScoped<IInventoryStatusService, InventoryStatusService>();
    builder.Services.AddScoped<IProductAssignmentService, ProductAssignmentService>();
    builder.Services.AddScoped<IOrderAndTasksCreationService, OrderAndTasksCreationService>();
    builder.Services.AddScoped<IOrderCancellationService, OrderCancellationService>();
    builder.Services.AddScoped<IBarcodeGeneratorService, BarcodeGeneratorService>();
    //====================================================================================================//
    
    
    
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //Set a legacy behaviour for timestamp handling in Npsql
    
    
    
      //====================================================================================================//
     //Configure the database connection//
    //=================================//
    var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(ConnectionString);
    //====================================================================================================//

    
    
    //====================================================================================================//
    //Map enum types to the corresponding types in Psql database//
    //=========================================================//
    dataSourceBuilder.MapEnum<ActionType>("action_type");
    dataSourceBuilder.MapEnum<AlertType>("alert_type");
    dataSourceBuilder.MapEnum<LevelType>("level_type");
    dataSourceBuilder.MapEnum<OrderName>("order_name");
    dataSourceBuilder.MapEnum<OrderType>("order_type");
    dataSourceBuilder.MapEnum<ReportType>("report_type");
    dataSourceBuilder.MapEnum<ReportPeriod>("report_period");
    //====================================================================================================//
    
    
    
    var dataSource = dataSourceBuilder.Build();//Build the source based on the settings

    
    
    //====================================================================================================//
    //Add services//
    //===========//
    builder.Services.AddDbContext<SmartwmsDbContext>(options => options.UseNpgsql(dataSource));//Add the database context
    builder.Services.AddAuthentication(options => //Adds authentication service
        {
            options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
        })
        .AddBearerToken(
            IdentityConstants.BearerScheme,
            conf =>
                conf.Events = new BearerTokenEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/notificationHub")))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                }
        )
        .AddIdentityCookies();
    
    builder.Services.AddIdentityCore<User>()
                    .AddApiEndpoints()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<SmartwmsDbContext>();//Adds Identity API endpoints with the roles and database stores
    builder.Services.AddHttpContextAccessor();
    
    builder.Services.AddAuthorization();//Adds authorization service
    
    //Configure CORS, to allow API access from any source
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyMethod()
                .AllowAnyHeader()
                //.AllowAnyOrigin()
                .AllowCredentials()
                .SetIsOriginAllowed(origin =>
                {
                    if (string.IsNullOrWhiteSpace(origin)) return false;
                    if (origin.ToLower().StartsWith("")) return true;
                    return false;
                });
        });
    });
    //====================================================================================================//

    
    
    var app = builder.Build();//Build the application based on current settings

    
    
      //====================================================================================================//
     //Create roles//
    //============//
    using (var scope = app.Services.CreateScope())
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
    //====================================================================================================//

      //==================================================================================================//
     //Create warehouse and countries//
    //================//

    using (var scope = app.Services.CreateScope())
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

        var countryInitializer = new CountryInitializer(dbContext);
        await countryInitializer.InitializeAsync();
    }
    
    //====================================================================================================//

      //====================================================================================================//
     //Admin user//
    //==========//
    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

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
                WarehousesWarehouseId = 1
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
    //====================================================================================================//
    
    
    

      //====================================================================================================//
     //Middleware pipeline configuraion//
    //===================================//
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapIdentityApi<User>();

    app.MapControllers();

    app.UseHttpsRedirection();

    app.MapHub<NotificationHub>("/notificationHub");

    app.UseHangfireDashboard();
    
    RecurringJob.AddOrUpdate<IInventoryStatusService>(
        "CheckInventoryStatus",
        service => service.CheckInventory(),
        Cron.Hourly);

    app.Run();
    //====================================================================================================//
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception"); // NLog: catch setup errors
    throw;
}
finally
{
    NLog.LogManager.Shutdown();// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
}

public partial class Program {}
