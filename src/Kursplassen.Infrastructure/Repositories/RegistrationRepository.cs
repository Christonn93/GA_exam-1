using Kursplassen.Core.Entities;
using Kursplassen.Core.Enums;
using Kursplassen.Core.Interfaces;
using Kursplassen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kursplassen.Infrastructure.Repositories;

public class RegistrationRepository : IRegistrationRepository
{
    // EF Core context used for all registration queries and writes
    protected readonly KursplassenDbContext _context;

    public RegistrationRepository(KursplassenDbContext context)
    {
        _context = context;
    }

    // Load a single registration by its ID; returns null when no matching row exists
    public async Task<Registration?> GetByIdAsync(Guid registrationId)
        => await _context.Registrations
            .FirstOrDefaultAsync(registration => registration.Id == registrationId);

    // Load every registration belonging to a participant,
    // newest first so callers can show the most recent sign-ups on top
    public async Task<IReadOnlyList<Registration>> GetByParticipantAsync(Guid participantUserId)
        => await _context.Registrations
            .Where(registration => registration.ParticipantUserId == participantUserId)
            // Most recent registrations first
            .OrderByDescending(registration => registration.RegisteredAtUtc)
            .ToListAsync();

    // Track and persist a new registration, then return the same instance
    // so callers can read any DB-generated values (e.g. Id)
    public async Task<Registration> AddAsync(Registration registration)
    {
        _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();
        return registration;
    }

    // Mark the given registration as modified, persist all changes,
    // and return it so callers can continue using the updated entity
    public async Task<Registration?> UpdateAsync(Registration registration)
    {
        // Mark entity as modified and persist
        _context.Registrations.Update(registration);
        await _context.SaveChangesAsync();
        return registration;
    }

    // Count active (non-cancelled) registrations for a course
    public async Task<int> GetActiveCountAsync(Guid courseId)
        => await _context.Registrations
            .CountAsync(registration =>
                registration.CourseId == courseId &&
                registration.Status == RegistrationStatus.Active);

    // Find the active registration for a given course and participant (null if none)
    public async Task<Registration?> GetActiveByCourseAndParticipantAsync(Guid courseId, Guid participantUserId)
        => await _context.Registrations
            .FirstOrDefaultAsync(registration =>
                registration.CourseId == courseId &&
                registration.ParticipantUserId == participantUserId &&
                registration.Status == RegistrationStatus.Active);
}