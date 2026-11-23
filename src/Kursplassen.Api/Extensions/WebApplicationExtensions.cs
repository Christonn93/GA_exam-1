using Kursplassen.Api.Middleware;
using Scalar.AspNetCore;

namespace Kursplassen.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiPipeline(
        this WebApplication app)
    {
        // Log every request first — outermost in the app pipeline, so it always
        // records the final status code (including after exception handling).
        app.UseMiddleware<RequestLoggingMiddleware>();

        // Convert unhandled exceptions to ProblemDetails. Placed inside the
        // request logger so the logger observes the rewritten status code.
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // Expose the OpenAPI document and both documentation UIs
        // only during development.
        if (app.Environment.IsDevelopment())
        {
            // Serve the generated OpenAPI JSON document
            app.MapOpenApi();

            // Swagger UI at /swagger, pointing at the OpenAPI document
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    "/openapi/v1.json",
                    "Kursplassen API v1");

                options.RoutePrefix = "swagger";
            });

            // Alternative modern API reference UI (Scalar)
            app.MapScalarApiReference();
        }

        // Enforce authorization for downstream endpoints.
        app.UseAuthorization();

        return app;
    }
}