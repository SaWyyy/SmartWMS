using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;
using NLog;
using Npgsql;
using SmartWMS.Entities;
using SmartWMS.Entities.Enums;
using SmartWMS.Models;
using SmartWMS.Repositories;
using SmartWMS.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Filters;

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
    builder.Services.AddTransient<IProductDetailRepository, ProductDetailRepository>();
    builder.Services.AddTransient<IAlertRepository, AlertRepository>();
    builder.Services.AddTransient<IProductRepository, ProductRepository>();
    builder.Services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
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
    builder.Services.AddAuthorization();//Adds authorization service
    builder.Services.AddAuthentication();//Adds authentication service
    builder.Services.AddIdentityApiEndpoints<User>().AddRoles<IdentityRole>().AddEntityFrameworkStores<SmartwmsDbContext>();//Adds Identity API endpoints with the roles and database stores
    builder.Services.AddHttpContextAccessor();
    
    //Configure CORS, to allow API access from any source
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin();
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
    
    
    
      //====================================================================================================//
     //Admin user//
    //==========//
    using (var scope = app.Services.CreateScope())
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
