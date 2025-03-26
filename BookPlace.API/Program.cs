
using Asp.Versioning;
using BookPlace.API.Configs;
using BookPlace.Core.Application.Interfaces;
using BookPlace.Core.Domain.Entities;
using BookPlace.Infrastructure.Data;
using BookPlace.Infrastructure.Mapping;
using BookPlace.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Register AutoMapper for mapping real model entities to DTOs and vice vers
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register DbContext with SQL Server Connection String
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:SqlServerConnection"])
);
// Add User Management
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
ConfigureServices(builder.Services);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); // This is to convert ENUM values to names (strings)

// Add Authentication services for JWT
builder.Services.AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opts =>
    {
        opts.RequireHttpsMetadata = false;
        opts.SaveToken = true;
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

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
SwaggerConfig.Initialize(builder.Services);

// Add authorization services
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    SwaggerConfig.Setup(app);
}

app.UseHttpsRedirection();

// Configure the HTTP request pipeline
app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization(); // Add authorization middleware

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IReservationsService, ReservationsService>();
    services.AddScoped<IUserService, UserService>();
}