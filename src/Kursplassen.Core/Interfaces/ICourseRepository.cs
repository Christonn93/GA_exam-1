using Kursplassen.Core.Entities;

namespace Kursplassen.Core.Interfaces;

public interface ICourseRepository
{
    // Fetch a single course by ID (null if not found)
    Task<Course?> GetByIdAsync(Guid courseId);

    // List published courses that haven't started yet
    Task<IReadOnlyList<Course>> GetUpcomingPublishedAsync();

    // Persist a new course and return the tracked entity
    Task<Course> AddAsync(Course course);

    // Persist changes to an existing course (null if not found)
    Task<Course?> UpdateAsync(Course course);

    // Remove a course by ID; returns true if a row was deleted
    Task<bool> DeleteAsync(Guid courseId);
}