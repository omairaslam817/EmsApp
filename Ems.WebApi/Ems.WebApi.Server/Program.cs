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
using Scrutor;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>(); // Handles JWT authentication in the API

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
builder.Services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>((sp, optionsBuilder) =>
{
    optionsBuilder.UseSqlServer(connectionString);
});

// Authentication and Authorization Configuration

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Configure JwtBearer options here if needed (e.g., Authority, Audience, etc.)
    });

builder.Services.AddAuthorization(options =>
{
    // Configure authorization policies if needed
});
// Add memory cache
builder.Services.AddMemoryCache();

// Add distributed cache (e.g., Redis)
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Adjust with your Redis server configuration
    options.InstanceName = "EmsCache_";
});

// Register the handler
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthrizationPolicyProvider>();

// Build the application

WebApplication app = builder.Build();

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
