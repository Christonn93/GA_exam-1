using Kursplassen.Core.DTOs.Users;
using Kursplassen.Core.Interfaces;

namespace Kursplassen.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(
        this IEndpointRouteBuilder app)
    {
        // Group user lookup routes under /api/user;
        // any authenticated user may look up a public profile
        var group = app.MapGroup("/api/user")
            .WithTags("Users")
            .RequireAuthorization();

        // GET /api/user/{userId} - public profile of a user (no email / password hash)
        group.MapGet("/{userId:guid}", async (
            Guid userId,
            IUserService userService) =>
        {
            var user = await userService.GetByIdAsync(userId);
            return Results.Ok(user);
        });

        return app;
    }
}
