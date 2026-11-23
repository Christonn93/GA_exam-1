namespace Kursplassen.Api.Endpoints;

public static class HealthEndpoints
{
    public static IEndpointRouteBuilder MapHealthEndpoints(
        this IEndpointRouteBuilder app)
    {
        // Group all health endpoints under /health
        var group = app.MapGroup("/health")
            .WithTags("Health");

        // Simple liveness probe returning static status
        group.MapGet("/", () => Results.Ok(new
            {
                message = "Kursplassen API is running",
                version = "v1",
                status = "Healthy!"
            }))
            .WithName("GetHealth")
            .WithSummary("Check whether the API is running");

        return app;
    }
}