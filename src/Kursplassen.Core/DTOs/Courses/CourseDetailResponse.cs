namespace Kursplassen.Core.DTOs.Courses;

public class CourseDetailResponse
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

    // Number of active registrations
    public int RegisteredCount { get; set; }

    // Capacity minus RegisteredCount (what's left to book)
    public int AvailableSpots { get; set; }
}