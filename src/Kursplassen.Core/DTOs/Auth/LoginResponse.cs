namespace Kursplassen.Core.DTOs.Auth;

public class LoginResponse
{
    // Signed JWT the client sends as a Bearer token on later requests
    public string Token { get; set; } = default!;

    // Identity of the authenticated user
    public Guid UserId { get; set; }
    public string DisplayName { get; set; } = default!;

    // User's role (e.g. Admin / Instructor / Student) for client-side UI decisions
    public string Role { get; set; } = default!;
}