using Kursplassen.Core.DTOs.Auth;
using Kursplassen.Core.Entities;

namespace Kursplassen.Core.Interfaces;

public interface IAuthService
{
    // Create a new user after validation and password hashing;
    // throws ConflictException when the email is already taken
    Task<User> RegisterAsync(string email, string displayName, string password);

    // Validate credentials and return a signed JWT + user info;
    // throws on invalid email/password
    Task<LoginResponse> LoginAsync(string email, string password);
}