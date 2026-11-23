
using Kursplassen.Core.Enums;
namespace Kursplassen.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public UserRole Role { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public ICollection<Course> OwnedCourses { get; set; } = new List<Course>();
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}