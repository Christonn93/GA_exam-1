namespace Kursplassen.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder app)
    {
        // Group all auth routes under /auth
        var group = app.MapGroup("/auth").WithTags("Auth");

        // GET fallbacks redirect to the main /auth page
        // (POST handlers live elsewhere or are added later)
        group.MapGet("/register", () => Results.Redirect("/auth"));
        group.MapGet("/login", () => Results.Redirect("/auth"));
        group.MapGet("/logout", () => Results.Redirect("/auth"));
        
        // JWT Token endpoints
        // (POST handlers live elsewhere or are added later)
        group.MapGet("/token", () => Results.Redirect("/auth"));
        group.MapGet("/refresh", () => Results.Redirect("/auth"));

        return app;
    }
}