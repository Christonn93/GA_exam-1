namespace Kursplassen.Core.DTOs.Courses;

public class CreateCourseRequest
{
    // Short course name shown in listings
    public string Title { get; set; } = default!;

    // Full course description (free text)
    public string Description { get; set; } = default!;

    // Course start time (UTC)
    public DateTime StartsAtUtc { get; set; }

    // Course end time (UTC); must be after StartsAtUtc
    public DateTime EndsAtUtc { get; set; }

    // Physical or virtual location
    public string Location { get; set; } = default!;

    // Maximum number of participants
    public int Capacity { get; set; }
}