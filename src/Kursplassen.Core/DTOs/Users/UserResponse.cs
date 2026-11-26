namespace Kursplassen.Core.DTOs.Users;

public class UserResponse
{
    public Guid Id { get; set; }

    // Public profile fields only (no email / password hash)
    public string DisplayName { get; set; } = default!;

    public string Role { get; set; } = default!; // e.g. "Participant" / "Instructor"

    public DateTime CreatedAtUtc { get; set; }
}
