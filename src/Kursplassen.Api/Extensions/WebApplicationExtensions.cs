using Kursplassen.Api.Middleware;

namespace Kursplassen.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiPipeline(
        this WebApplication app)
    {
        // Expose OpenAPI doc only in development
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    "/openapi/v1.json",
                    "Kursplassen API v1");

                options.RoutePrefix = "swagger";
            });
        }

        // Log every request (registered early to capture full pipeline duration)
        app.UseMiddleware<RequestLoggingMiddleware>();

        // Enforce authorization for downstream endpoints
        app.UseAuthorization();

        return app;
    }
}