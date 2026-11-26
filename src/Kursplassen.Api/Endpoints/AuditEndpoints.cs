using Kursplassen.Core.DTOs.Audit;
using Kursplassen.Core.Enums;
using Kursplassen.Core.Interfaces;

namespace Kursplassen.Api.Endpoints;

public static class AuditEndpoints
{
    public static IEndpointRouteBuilder MapAuditEndpoints(
        this IEndpointRouteBuilder app)
    {
        // Audit endpoints are restricted to instructors
        var group = app.MapGroup("/api/audit")
            .WithTags("Audit")
            .RequireAuthorization(policy => policy.RequireRole("Instructor"));

        // GET /api/audit - list audit entries, optionally filtered by type, newest first
        group.MapGet("/", async (IAuditService auditService, AuditLogType? type) =>
        {
            var logs = await auditService.GetAllAsync(type);
            return Results.Ok(logs);
        });

        // GET /api/audit/{auditId} - fetch a single audit entry
        group.MapGet("/{auditId:guid}", async (
            Guid auditId,
            IAuditService auditService) =>
        {
            var log = await auditService.GetByIdAsync(auditId);
            return Results.Ok(log);
        });

        return app;
    }
}
