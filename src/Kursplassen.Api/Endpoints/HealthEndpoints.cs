namespace Kursplassen.Api.Endpoints;

public static class HealthEndpoints
{
    // Captured once at startup to compute uptime
    private static readonly DateTimeOffset ServerStartedAtUtc = DateTimeOffset.UtcNow;

    public static IEndpointRouteBuilder MapHealthEndpoints(
        this IEndpointRouteBuilder app)
    {
        // Group all health endpoints under /health
        var group = app.MapGroup("/health")
            .WithTags("Health");

        group.MapGet("/", () =>
            {
                var requestTimeUtc = DateTimeOffset.UtcNow;

                // Time elapsed since the app started
                var uptime = requestTimeUtc - ServerStartedAtUtc;

                // Resolve Norwegian time zone once for both timestamps
                var norwegianTimeZone = TimeZoneInfo
                    .FindSystemTimeZoneById("Europe/Oslo");

                // Local-time projections of the UTC timestamps
                var serverStartedAtNorway = TimeZoneInfo.ConvertTime(
                    ServerStartedAtUtc,
                    norwegianTimeZone);

                var requestTimeNorway = TimeZoneInfo.ConvertTime(
                    requestTimeUtc,
                    norwegianTimeZone);

                return Results.Ok(new
                {
                    status = "Healthy",
                    message = "Kursplassen API is running",
                    version = "v1",

                    serverStartedAtUtc = ServerStartedAtUtc,
                    serverStartedAtNorway = serverStartedAtNorway,

                    requestTimeUtc = requestTimeUtc,
                    requestTimeNorway = requestTimeNorway,

                    uptime = new
                    {
                        days = uptime.Days,
                        hours = uptime.Hours,
                        minutes = uptime.Minutes,
                        seconds = uptime.Seconds,

                        // e.g. "1d 2h 3m 4s"
                        humanReadable =
                            $"{uptime.Days}d {uptime.Hours}h " +
                            $"{uptime.Minutes}m {uptime.Seconds}s"
                    }
                });
            })
            .WithName("GetHealth")
            .WithSummary("Check whether the API is running");

        return app;
    }
}