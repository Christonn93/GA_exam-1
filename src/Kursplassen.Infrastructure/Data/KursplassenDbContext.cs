using Kursplassen.Core.Entities;
using Kursplassen.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Kursplassen.Infrastructure.Data;

public class KursplassenDbContext(DbContextOptions<KursplassenDbContext> options) : DbContext(options)
{
    // Expose tables via DbSet; Set<T>() avoids backing-field boilerplate
    public DbSet<User> Users => Set<User>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Registration> Registrations => Set<Registration>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply entity type configurations (table names, keys, column constraints)
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new RegistrationConfiguration());

        // Course (aggregate root) -> Owner (User)
        modelBuilder.Entity<Course>()
            .HasOne(course => course.Owner)
            .WithMany(user => user.OwnedCourses)
            .HasForeignKey(course => course.OwnerUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Registration -> Course
        modelBuilder.Entity<Registration>()
            .HasOne(registration => registration.Course)
            .WithMany(course => course.Registrations)
            .HasForeignKey(registration => registration.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Registration -> Participant (User)
        modelBuilder.Entity<Registration>()
            .HasOne(registration => registration.Participant)
            .WithMany(user => user.Registrations)
            .HasForeignKey(registration => registration.ParticipantUserId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}