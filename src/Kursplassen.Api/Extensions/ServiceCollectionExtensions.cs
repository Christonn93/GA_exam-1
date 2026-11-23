using Kursplassen.Api.Endpoints;
using Kursplassen.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql;

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

        // Register the EF Core DbContext backed by MySQL
        // (MySqlServerVersion pins the provider behavior to MySQL 8.0)
        services.AddDbContext<KursplassenDbContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("Kursplassen"),
                new MySqlServerVersion(new Version(8, 0))));

        return services;
    }

    public static IServiceCollection AddApiServices(
        this IServiceCollection services)
    {
        // Enable MVC/controller-based endpoints
        services.AddControllers();

        // Register OpenAPI document generation (served in development)
        services.AddOpenApi();

        return services;
    }
}