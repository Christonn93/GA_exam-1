namespace Kursplassen.Core.DTOs.Registrations;

public class RegistrationResponse
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string CourseTitle { get; set; } = default!;
    public Guid ParticipantUserId { get; set; }
    public string Status { get; set; } = default!; // "Active" | "Cancelled"

    public DateTime RegisteredAtUtc { get; set; }
    public DateTime? CancelledAtUtc { get; set; }
}
