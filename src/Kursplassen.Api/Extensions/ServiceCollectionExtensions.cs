using Kursplassen.Api.Endpoints;

namespace Kursplassen.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        // Register application/domain services here.
        // Example:
        // services.AddScoped<ICourseService, CourseService>();

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