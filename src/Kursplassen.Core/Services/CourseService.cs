using Kursplassen.Core.DTOs.Courses;
using Kursplassen.Core.Entities;
using Kursplassen.Core.Enums;
using Kursplassen.Core.Exceptions;
using Kursplassen.Core.Interfaces;

namespace Kursplassen.Core.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IRegistrationRepository _registrationRepository;

    public CourseService(
        ICourseRepository courseRepository,
        IRegistrationRepository registrationRepository)
    {
        _courseRepository = courseRepository;
        _registrationRepository = registrationRepository;
    }

    public async Task<IReadOnlyList<CourseResponse>> GetUpcomingPublishedAsync()
    {
        // Repository already filters by status/date and sorts by start time
        var courses = await _courseRepository.GetUpcomingPublishedAsync();
        return courses.Select(MapToResponse).ToList();
    }

    public async Task<CourseDetailResponse> GetDetailAsync(Guid courseId)
    {
        // Missing course -> 404 via NotFoundException
        var course = await _courseRepository.GetByIdAsync(courseId)
            ?? throw new NotFoundException("Course not found.");

        // Active registrations drive the "spots left" calculation
        var activeCount = await _registrationRepository.GetActiveCountAsync(courseId);

        return new CourseDetailResponse
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            StartsAtUtc = course.StartsAtUtc,
            EndsAtUtc = course.EndsAtUtc,
            Location = course.Location,
            Capacity = course.Capacity,
            Status = course.Status.ToString(),
            RegisteredCount = activeCount,
            // Clamp at 0 in case capacity was lowered below current registrations
            AvailableSpots = Math.Max(0, course.Capacity - activeCount)
        };
    }

    public async Task<CourseResponse> CreateAsync(Guid ownerUserId, CreateCourseRequest request)
    {
        var now = DateTime.UtcNow;

        var course = new Course
        {
            // App-generated ID so the entity is valid before persisting
            Id = Guid.NewGuid(),
            OwnerUserId = ownerUserId,
            Title = request.Title,
            Description = request.Description,
            StartsAtUtc = request.StartsAtUtc,
            EndsAtUtc = request.EndsAtUtc,
            Location = request.Location,
            Capacity = request.Capacity,

            // New courses start as drafts; the owner publishes them via update.
            Status = CourseStatus.Draft,

            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        var saved = await _courseRepository.AddAsync(course);
        return MapToResponse(saved);
    }

    public async Task<CourseResponse> UpdateAsync(Guid ownerUserId, Guid courseId, UpdateCourseRequest request)
    {
        var course = await _courseRepository.GetByIdAsync(courseId)
            ?? throw new NotFoundException("Course not found.");

        // Ownership check -> 403 if another user tries to edit
        if (course.OwnerUserId != ownerUserId)
        {
            throw new ForbiddenException("You can only update courses you own.");
        }

        course.Title = request.Title;
        course.Description = request.Description;
        course.StartsAtUtc = request.StartsAtUtc;
        course.EndsAtUtc = request.EndsAtUtc;
        course.Location = request.Location;
        course.Capacity = request.Capacity;
        course.Status = request.Status;

        // Bump the audit timestamp on every update
        course.UpdatedAtUtc = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
        return MapToResponse(course);
    }

    public async Task DeleteAsync(Guid ownerUserId, Guid courseId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId)
            ?? throw new NotFoundException("Course not found.");

        // Ownership check -> 403 if another user tries to delete
        if (course.OwnerUserId != ownerUserId)
        {
            throw new ForbiddenException("You can only delete courses you own.");
        }

        // A course with active registrations cannot be deleted (requirement 7).
        var activeCount = await _registrationRepository.GetActiveCountAsync(courseId);
        if (activeCount > 0)
        {
            throw new ConflictException("Cannot delete a course with active registrations.");
        }

        await _courseRepository.DeleteAsync(courseId);
    }

    // Shared projection from entity to the lightweight list-view DTO
    private static CourseResponse MapToResponse(Course course) => new()
    {
        Id = course.Id,
        Title = course.Title,
        Description = course.Description,
        StartsAtUtc = course.StartsAtUtc,
        EndsAtUtc = course.EndsAtUtc,
        Location = course.Location,
        Capacity = course.Capacity,
        Status = course.Status.ToString()
    };
}