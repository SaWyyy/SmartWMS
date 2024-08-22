using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using SmartWMS.Models;
using SmartWMS.Models.Enums;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dataSourceBuilder = new NpgsqlDataSourceBuilder(ConnectionString);

dataSourceBuilder.MapEnum<ActionType>("action_type");
dataSourceBuilder.MapEnum<AlertType>("alert_type");
dataSourceBuilder.MapEnum<LevelType>("level_type");
dataSourceBuilder.MapEnum<OrderName>("order_name");
dataSourceBuilder.MapEnum<OrderType>("order_type");
dataSourceBuilder.MapEnum<ReportType>("report_type");
dataSourceBuilder.MapEnum<ReportPeriod>("report_period");

var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<SmartwmsDbContext>(options =>
    options.UseNpgsql(dataSource));
/*
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<SmartwmsDbContext>()
    .AddDefaultTokenProviders();
*/

builder.Services.AddAuthorization();

builder.Services.AddAuthentication();

builder.Services.AddIdentityApiEndpoints<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SmartwmsDbContext>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("*")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapIdentityApi<User>();

app.UseHttpsRedirection();

app.Run();