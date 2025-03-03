
using Asp.Versioning;
using BookPlace.Core.Application.Interfaces;
using BookPlace.Infrastructure.Data;
using BookPlace.Infrastructure.Mapping;
using BookPlace.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Register AutoMapper for mapping real model entities to DTOs and vice vers
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register DbContext with SQL Server Connection String
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"))
);

// Add services to the container.
ConfigureServices(builder.Services);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); // This is to convert ENUM values to names (strings)

// Add API versioning
var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
{
    // Set default API version
    options.DefaultApiVersion = new ApiVersion(1, 0);

    // Assume version 1.0 unless stated explicitly in the URL
    options.AssumeDefaultVersionWhenUnspecified = true;

    // Advertise the API versions supported by the app
    options.ReportApiVersions = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IReservationsService, ReservationsService>();
}