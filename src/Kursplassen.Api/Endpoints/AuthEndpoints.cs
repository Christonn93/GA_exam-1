using Kursplassen.Core.DTOs.Auth;
using Kursplassen.Core.Interfaces;
using Kursplassen.Api.Validation;

namespace Kursplassen.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder app)
    {
        // Group all auth routes under /api/auth
        var group = app.MapGroup("/api/auth")
            .WithTags("Auth")
            .AddEndpointFilter<ValidationFilter>();

        // POST /api/auth/register - public registration, server assigns Participant role
        group.MapPost("/register", async (RegisterRequest request, IAuthService authService) =>
        {
            var user = await authService.RegisterAsync(
                request.Email, request.DisplayName, request.Password);

            return Results.Created($"/api/auth/register/{user.Id}", new { user.Id, user.DisplayName, user.Role });
        });

        // POST /api/auth/login - validates credentials and returns a JWT
        group.MapPost("/login", async (LoginRequest request, IAuthService authService) =>
        {
            var response = await authService.LoginAsync(request.Email, request.Password);
            return Results.Ok(response);
        });

        // POST /api/auth/token - issues a JWT for the supplied credentials
        group.MapPost("/token", async (LoginRequest request, IAuthService authService) =>
        {
            var response = await authService.LoginAsync(request.Email, request.Password);
            return Results.Ok(response);
        });

        // POST /api/auth/token/refresh - refreshes the token for the supplied credentials
        group.MapPost("/token/refresh", async (LoginRequest request, IAuthService authService) =>
        {
            var response = await authService.LoginAsync(request.Email, request.Password);
            return Results.Ok(response);
        });

        return app;
    }
}
