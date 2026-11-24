namespace Kursplassen.Core.DTOs.Courses;

public class CourseResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    // Course times (UTC)
    public DateTime StartsAtUtc { get; set; }
    public DateTime EndsAtUtc { get; set; }

    public string Location { get; set; } = default!;

    // Maximum number of participants
    public int Capacity { get; set; }

    // Lifecycle state as a string (e.g. "Published")
    public string Status { get; set; } = default!;
}