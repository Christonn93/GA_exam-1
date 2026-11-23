using Kursplassen.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kursplassen.Infrastructure.Configurations;

public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
{
    public void Configure(EntityTypeBuilder<Registration> builder)
    {
        // Map entity to the "registrations" table
        builder.ToTable("registrations");

        builder.HasKey(registration => registration.Id);

        // Course (FK -> courses.id, char(36))
        builder.Property(registration => registration.CourseId).HasColumnType("char(36)").IsRequired();

        // Participant (FK -> users.id, char(36))
        builder.Property(registration => registration.ParticipantUserId).HasColumnType("char(36)").IsRequired();

        // Registration timestamp is always set
        builder.Property(registration => registration.RegisteredAtUtc).IsRequired();

        // Cancellation timestamp is only set when cancelled
        builder.Property(registration => registration.CancelledAtUtc).IsRequired(false);

        // Nullable "shadow" column used for uniqueness (e.g. one active registration per user/course)
        builder.Property(registration => registration.ActiveParticipantUserId).IsRequired(false);
    }
}