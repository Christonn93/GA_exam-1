using Kursplassen.Api.Middleware;
using Scalar.AspNetCore;

namespace Kursplassen.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiPipeline(
        this WebApplication app)
    {
        // Log requests, including errors and rate-limit rejections.
        app.UseMiddleware<RequestLoggingMiddleware>();

        // Convert unhandled exceptions into ProblemDetails responses.
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // Expose API documentation only during development.
        if (app.Environment.IsDevelopment())
        {
            // OpenAPI JSON document.
            app.MapOpenApi();

            // Swagger UI at /swagger.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    "/openapi/v1.json",
                    "Kursplassen API v1");

                options.RoutePrefix = "swagger";
            });

            // Scalar UI at /scalar.
            app.MapScalarApiReference();
        }

        // Resolve the endpoint and its metadata.
        app.UseRouting();

        // Populate HttpContext.User before applying per-user limits.
        app.UseAuthentication();

        // Apply the rate limiter registered in AddApiServices().
        app.UseRateLimiter();

        // Enforce authorization for the selected endpoint.
        app.UseAuthorization();

        return app;
    }
}