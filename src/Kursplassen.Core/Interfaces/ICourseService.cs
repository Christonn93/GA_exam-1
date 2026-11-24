using Kursplassen.Core.DTOs.Courses;

namespace Kursplassen.Core.Interfaces;

public interface ICourseService
{
    // List published, upcoming courses for the public catalogue
    Task<IReadOnlyList<CourseResponse>> GetUpcomingPublishedAsync();

    // Get a single course's details including registration counts
    Task<CourseDetailResponse> GetDetailAsync(Guid courseId);

    // Create a new course owned by the given instructor
    Task<CourseResponse> CreateAsync(Guid ownerUserId, CreateCourseRequest request);

    // Update a course; only its owner may do so
    Task<CourseResponse> UpdateAsync(Guid ownerUserId, Guid courseId, UpdateCourseRequest request);

    // Delete a course; only its owner may do so, and only if it has no active registrations
    Task DeleteAsync(Guid ownerUserId, Guid courseId);
}