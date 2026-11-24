using Kursplassen.Core.Enums;

namespace Kursplassen.Core.Interfaces;

public interface ICurrentUserService
{
    // True when the request carries a valid authenticated principal
    bool IsAuthenticated { get; }

    // Authenticated user's id, or Guid.Empty when anonymous
    Guid UserId { get; }

    // Authenticated user's role, or UserRole.Participant when anonymous
    UserRole Role { get; }
}