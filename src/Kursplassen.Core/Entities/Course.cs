using Kursplassen.Core.Enums;

namespace Kursplassen.Api.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartsAtUtc { get; set; }
    public DateTime EndsAtUtc { get; set; }
    public string Location { get; set; } = default!;
    public int Capacity { get; set; }
    public CourseStatus Status { get; set; }              // Draft | Published | Cancelled
    public int InstructorId { get; set; }                 // FK → User (owner)
    public User Instructor { get; set; } = default!;
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}