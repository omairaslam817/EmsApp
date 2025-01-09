using Ems.Application.Abstractions.Jwt;
using Ems.Application.AutoMapper;
using Ems.Application.Behaviors;
using Ems.Domain.Interfaces;
using Ems.Domain.Repositories;
using Ems.Infrastructure.Authorization;
using Ems.Infrastructure.Authrization;
using Ems.Persistence;
using Ems.Persistence.Interceptors;
using Ems.Persistence.Repositories;
using Ems.Persistence.Services;
using Ems.WebApi.Server.OptionsSetup;
using FluentValidation;
using Gatherly.Persistence.Interceptors;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Add In-Memory Cache
builder.Services.AddMemoryCache();

// Dependency Injection Configuration

builder.Services.AddScoped<IMemberRepository, MemberRepository>();

builder.Services.Scan(selector =>
    selector.FromAssemblies(
            Ems.Infrastructure.AssemblyReference.Assembly,
            Ems.Persistence.AssemblyReference.Assembly)
        .AddClasses(false)
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        .AsMatchingInterface()
        .WithScopedLifetime());

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Ems.Application.AssemblyReference.Assembly));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

builder.Services.AddControllers().AddApplicationPart(Ems.Presentation.AssemblyReference.Assembly);

// Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
    // Swagger configuration for JWT token inclusion
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter 'Bearer' followed by your JWT token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Configure options for JWT Bearer and other JWT settings
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>(); // Handles JWT authentication in the API

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
builder.Services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>((sp, optionsBuilder) =>
{
    optionsBuilder.UseSqlServer(connectionString);
});

// Authentication and Authorization Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Configure JWT Bearer options here if needed (e.g., Authority, Audience, etc.)
    });

builder.Services.AddAuthorization(options =>
{
    // Configure authorization policies if needed
});

builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthrizationPolicyProvider>();
var redisConfig = builder.Configuration.GetSection("RedisCacheSettings").Get<RedisCacheSettings>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "AppName_";
});





// Build the application
var app = builder.Build();

// Middleware Configuration

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public class RedisCacheSettings
{
    public string ConnectionString { get; set; }
    public string InstanceName { get; set; }
}
