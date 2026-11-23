namespace Kursplassen.Core.DTOs.Auth;

public class RegisterRequest
{
    // Unique account email; also the login identifier
    public string Email { get; set; } = default!;

    // Plaintext password at the boundary, hashed before persisting
    public string Password { get; set; } = default!;

    // Human-friendly name shown in the UI
    public string DisplayName { get; set; } = default!;
}