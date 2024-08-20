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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.Run();