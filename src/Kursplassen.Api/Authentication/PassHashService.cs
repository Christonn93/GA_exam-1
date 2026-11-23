using Kursplassen.Core.Interfaces;
using BCrypt.Net;

namespace Kursplassen.Api.Authentication;

public class PassHashService : IPassHashService
{
    // Hash the plaintext password with BCrypt (salt + work factor embedded in the hash)
    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    // Constant-time comparison of plaintext against the stored BCrypt hash
    public bool Verify(string password, string passwordHash)
        => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}