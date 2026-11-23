using Kursplassen.Core.Entities;
using Kursplassen.Core.Enums;
using Kursplassen.Core.Interfaces;
using Kursplassen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kursplassen.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    // EF Core context used for all course queries and writes
    protected readonly KursplassenDbContext _context;

    public CourseRepository(KursplassenDbContext context)
    {
        _context = context;
    }

    // Load a single course by its ID; returns null when no matching row exists
    public async Task<Course?> GetByIdAsync(Guid courseId)
        => await _context.Courses
            .FirstOrDefaultAsync(course => course.Id == courseId);

    // Load published courses that haven't started yet,
    // ordered so the soonest upcoming course appears first
    public async Task<IReadOnlyList<Course>> GetUpcomingPublishedAsync()
        => await _context.Courses
            // Only published courses that start in the future
            .Where(course => course.Status == CourseStatus.Published && course.StartsAtUtc > DateTime.UtcNow)
            // Soonest first
            .OrderBy(course => course.StartsAtUtc)
            .ToListAsync();

    // Track and persist a new course, then return it so callers
    // can read DB-generated values (e.g. Id)
    public async Task<Course> AddAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    // Mark the given course as modified, persist all changes,
    // and return it so callers can continue using the updated entity
    public async Task<Course?> UpdateAsync(Course course)
    {
        // Mark entity as modified and persist
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
        return course;
    }

    // Delete a course by ID; returns false when the course doesn't exist
    public async Task<bool> DeleteAsync(Guid courseId)
    {
        var course = await GetByIdAsync(courseId);

        // Nothing to delete -> report failure
        if (course is null)
        {
            return false;
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        return true;
    }
}