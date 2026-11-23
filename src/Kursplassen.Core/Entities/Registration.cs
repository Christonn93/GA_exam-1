using Kursplassen.Core.Enums;

namespace Kursplassen.Core.Entities;

public class Registration
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public RegistrationStatus Status { get; set; }        // Active | Cancelled
    public DateTime CreatedAtUtc { get; set; }
}