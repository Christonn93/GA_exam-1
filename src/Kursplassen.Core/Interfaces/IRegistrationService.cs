using Kursplassen.Core.DTOs.Registrations;

namespace Kursplassen.Core.Interfaces;

public interface IRegistrationService
{
    // Register a participant for a course, enforcing capacity, publication, start time and duplicate rules
    Task<RegistrationResponse> RegisterAsync(Guid courseId, Guid participantUserId);

    // List the participant's own registrations with course information
    Task<IReadOnlyList<RegistrationResponse>> GetMineAsync(Guid participantUserId);

    // Cancel a participant's own registration and free the spot
    Task CancelAsync(Guid registrationId, Guid participantUserId);
}
