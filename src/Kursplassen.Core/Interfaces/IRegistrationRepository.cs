using Kursplassen.Core.Entities;

namespace Kursplassen.Core.Interfaces;

public interface IRegistrationRepository
{
    // Fetch a single registration by ID (null if not found)
    Task<Registration?> GetByIdAsync(Guid registrationId);

    // List all registrations belonging to a participant
    Task<IReadOnlyList<Registration>> GetByParticipantAsync(Guid participantUserId);

    // Persist a new registration and return the tracked entity
    Task<Registration> AddAsync(Registration registration);

    // Persist changes to an existing registration (null if not found)
    Task<Registration?> UpdateAsync(Registration registration);
}