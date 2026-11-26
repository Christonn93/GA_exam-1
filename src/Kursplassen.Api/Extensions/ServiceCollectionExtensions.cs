using Kursplassen.Api.Authentication;
using Kursplassen.Api.Endpoints;
using Kursplassen.Api.Middleware;
using Kursplassen.Api.Validation;
using Kursplassen.Core.Interfaces;
using Kursplassen.Core.Services;
using Kursplassen.Core.Validation;
using Kursplassen.Infrastructure.Data;
using Kursplassen.Infrastructure.Repositories;

using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kursplassen.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register application/domain services here.
        // Example:
        // services.AddScoped<ICourseService, CourseService>();

        // Register the EF Core DbContext backed by SQLite
        services.AddDbContext<KursplassenDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("Kursplassen")));

        // Register application/domain services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IRegistrationService, RegistrationService>();

        // Register data-access repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();

        // Register audit log repository
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();

        // Register user service
        services.AddScoped<IUserService, UserService>();

        // Register audit service
        services.AddScoped<IAuditService, AuditService>();

        // Register authentication infrastructure
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPassHashService, PassHashService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Required by CurrentUserService to read the authenticated principal
        services.AddHttpContextAccessor();

        // Register OpenAPI document generation (served in development)
        services.AddOpenApi();

        // Bind JwtOptions from configuration section "JWT"
        services.Configure<JwtOptions>(configuration.GetSection("JWT"));

        return services;
    }

    public static IServiceCollection AddApiServices(
        this IServiceCollection services)
    {
        // Enable MVC/controller-based endpoints
        services.AddControllers();

        // Register OpenAPI document generation (served in development)
        services.AddOpenApi();

        // Register FluentValidation validators (from Core assembly)
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        // Register the endpoint validation filter
        services.AddTransient<ValidationFilter>();

        // Register rate limiting.
        services.AddRateLimiter(RateLimitingConfiguration.Configure);
        
        return services;
    }
}