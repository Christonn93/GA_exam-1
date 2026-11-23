using Kursplassen.Core.DTOs.Auth;
using Kursplassen.Core.Entities;
using Kursplassen.Core.Enums;
using Kursplassen.Core.Exceptions;
using Kursplassen.Core.Interfaces;

namespace Kursplassen.Core.Services;

public class AuthService : IAuthService
{
    // Persistence for user lookups and inserts
    private readonly IUserRepository _userRepository;

    // Builds signed JWTs after successful login
    private readonly IJwtTokenService _jwtTokenService;

    // Hashes and verifies passwords (BCrypt)
    private readonly IPassHashService _passHashService;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IPassHashService passHashService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _passHashService = passHashService;
    }

    public async Task<User> RegisterAsync(string email, string displayName, string password)
    {
        // Email is the unique login identifier, so refuse duplicates early
        // with a 409-mapped ConflictException instead of hitting a DB constraint
        if (await _userRepository.GetByEmailAsync(email) is not null)
        {
            throw new ConflictException("An account with this email already exists.");
        }

        var user = new User
        {
            // App-generated ID so the entity is valid before hitting the DB
            Id = Guid.NewGuid(),
            Email = email,
            DisplayName = displayName,

            // Hash before persisting — the plaintext password never reaches the DB
            PasswordHash = _passHashService.Hash(password),

            // Self-registered accounts always start with the lowest-privilege role;
            // admins must promote them later
            Role = UserRole.Participant,

            CreatedAtUtc = DateTime.UtcNow
        };

        // Persist and return the stored user (same instance, DB may fill defaults)
        return await _userRepository.AddAsync(user);
    }

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        // Deliberately use one error for both "unknown email" and "wrong password"
        // so callers can't enumerate which emails are registered.
        // Verify() runs even conceptually on the same code path (BCrypt is constant-time).
        if (user is null || !_passHashService.Verify(password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        // Credentials are valid — issue a signed JWT with the user's claims
        var token = await _jwtTokenService.GenerateTokenAsync(user);

        // Shape the response the API returns to the client
        return new LoginResponse
        {
            Token = token,
            UserId = user.Id,
            DisplayName = user.DisplayName,
            Role = user.Role.ToString()
        };
    }
}