using Kursplassen.Api.Validation;
using Kursplassen.Core.DTOs.Courses;
using Kursplassen.Core.Interfaces;

namespace Kursplassen.Api.Endpoints;

public static class CoursesEndpoints
{
    public static IEndpointRouteBuilder MapCoursesEndpoints(
        this IEndpointRouteBuilder app)
    {
        // Group all course routes under /api/courses
        var group = app.MapGroup("/api/courses")
            .WithTags("Courses")
            .AddEndpointFilter<ValidationFilter>();

        // GET /api/courses - public list of published, upcoming courses
        group.MapGet("/", async (ICourseService courseService) =>
        {
            var courses = await courseService.GetUpcomingPublishedAsync();
            return Results.Ok(courses);
        });

        // GET /api/courses/{courseId} - public course detail with counts
        group.MapGet("/{courseId:guid}", async (Guid courseId, ICourseService courseService) =>
        {
            var detail = await courseService.GetDetailAsync(courseId);
            return Results.Ok(detail);
        });

        // POST /api/courses - instructor creates a course (becomes the owner)
        group.MapPost("/", async (
            CreateCourseRequest request,
            ICourseService courseService,
            ICurrentUserService currentUser) =>
        {
            var response = await courseService.CreateAsync(currentUser.UserId, request);
            return Results.Created($"/api/courses/{response.Id}", response);
        }).RequireAuthorization(policy => policy.RequireRole("Instructor"));

        // PUT /api/courses/{courseId} - owner updates own course
        group.MapPut("/{courseId:guid}", async (
            Guid courseId,
            UpdateCourseRequest request,
            ICourseService courseService,
            ICurrentUserService currentUser) =>
        {
            var response = await courseService.UpdateAsync(currentUser.UserId, courseId, request);
            return Results.Ok(response);
        }).RequireAuthorization(policy => policy.RequireRole("Instructor"));

        // DELETE /api/courses/{courseId} - owner deletes own course (no active registrations)
        group.MapDelete("/{courseId:guid}", async (
            Guid courseId,
            ICourseService courseService,
            ICurrentUserService currentUser) =>
        {
            await courseService.DeleteAsync(currentUser.UserId, courseId);
            return Results.NoContent();
        }).RequireAuthorization(policy => policy.RequireRole("Instructor"));

        // POST /api/courses/{courseId}/registrations - participant registers for the course
        group.MapPost("/{courseId:guid}/registrations", async (
            Guid courseId,
            ICurrentUserService currentUser,
            IRegistrationService registrationService) =>
        {
            var response = await registrationService.RegisterAsync(courseId, currentUser.UserId);
            return Results.Created($"/api/courses/{courseId}/registrations/{response.Id}", response);
        }).RequireAuthorization(policy => policy.RequireRole("Participant"));

        return app;
    }
}
