using Kursplassen.Core.Enums;

namespace Kursplassen.Core.Entities;

public class Course
{
    public Guid Id { get; set; }
    public Guid OwnerUserId { get; set; }
    public User Owner { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartsAtUtc { get; set; }
    public DateTime EndsAtUtc { get; set; }
    public string Location { get; set; } = default!;
    public int Capacity { get; set; }
    public CourseStatus Status { get; set; }              // Draft | Published | Cancelled
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}