using Kursplassen.Core.Entities;

namespace Kursplassen.Core.Interfaces;

public interface IJwtTokenService
{
    // Build a signed JWT for the given user (claims, expiry, signing key)
    Task<string> GenerateTokenAsync(User user);
}