using AutoMapper;
using BeerBarBrewery.Mapping;
using BeerBarBrewery.MiddleWare;
using Business.BeerBarBrewery.Mapping;
using Business.BeerBarBrewery.Process;
using Business.BeerBarBrewery.Process.Interface;
using Database.BeerBarBrewery;
using Database.BeerBarBrewery.Repository;
using Database.BeerBarBrewery.Repository.Interface;
using Database.BeerBarBrewery.UnitOfWork;
using Database.BeerBarBrewery.UnitOfWork.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------
// Service Registration Section
// --------------------------------------------------------

// Add controller services (MVC pattern)
builder.Services.AddControllers();

// Register Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BeerConnectionString")));

// Register repositories for data access layer (Dependency Injection)
builder.Services.AddScoped<IBreweryRepository, BreweryRepository>();
builder.Services.AddScoped<IBeerRepository, BeerRepository>();
builder.Services.AddScoped<IBarRepository, BarRepository>();

// Register business logic/services
builder.Services.AddScoped<IBeerProcess, BeerProcess>();
builder.Services.AddScoped<IBarProcess, BarProcess>();
builder.Services.AddScoped<IBreweryProcess, BreweryProcess>();

// Register Unit of Work pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register AutoMapper and add mapping profiles from assemblies
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ContractToModelMapping).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ModelToDataEntityMapping).Assembly));

// --------------------------------------------------------
// Build the app and configure middleware pipeline
// --------------------------------------------------------
var app = builder.Build();

// Enable Swagger UI only in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Authorization middleware (add UseAuthentication() before this if needed)
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Run the application
app.Run();

