using Kursplassen.Core.Interfaces;

namespace Kursplassen.Api.Endpoints;

public static class RegistrationsEndpoints
{
    public static IEndpointRouteBuilder MapRegistrationsEndpoints(
        this IEndpointRouteBuilder app)
    {
        // Group all registration routes under /api/registrations
        var group = app.MapGroup("/api/registrations").WithTags("Registrations");

        // GET /api/registrations/mine - the signed-in participant's own registrations
        group.MapGet("/mine", async (
            ICurrentUserService currentUser,
            IRegistrationService registrationService) =>
        {
            var registrations = await registrationService.GetMineAsync(currentUser.UserId);
            return Results.Ok(registrations);
        }).RequireAuthorization(policy => policy.RequireRole("Participant"));

        // DELETE /api/registrations/{registrationId} - the participant cancels their own registration
        group.MapDelete("/{registrationId:guid}", async (
            Guid registrationId,
            ICurrentUserService currentUser,
            IRegistrationService registrationService) =>
        {
            await registrationService.CancelAsync(registrationId, currentUser.UserId);
            return Results.NoContent();
        }).RequireAuthorization(policy => policy.RequireRole("Participant"));

        return app;
    }
}
