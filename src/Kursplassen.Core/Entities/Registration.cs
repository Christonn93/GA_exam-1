using Kursplassen.Core.Enums;

namespace Kursplassen.Core.Entities;

public class Registration
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public Guid ParticipantUserId { get; set; }
    public User Participant { get; set; } = default!;
    public RegistrationStatus Status { get; set; }        // Active | Cancelled
    public DateTime RegisteredAtUtc { get; set; }
    public DateTime? CancelledAtUtc { get; set; }
    public Guid? ActiveParticipantUserId { get; set; }
}