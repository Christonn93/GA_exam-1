using Kursplassen.Core.Enums;

namespace Kursplassen.Api.Models;

public class User
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public UserRole Role { get; set; }
    public ICollection<Course> OwnedCourses { get; set; } = new List<Course>();
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}