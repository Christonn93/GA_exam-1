namespace Kursplassen.Core.DTOs.Auth;

public class LoginRequest
{
    // User's email address (login identifier)
    public string Email { get; set; } = default!;

    // Plaintext password, validated against the stored hash
    public string Password { get; set; } = default!;
}