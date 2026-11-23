using Kursplassen.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kursplassen.Infrastructure.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        // Map entity to the "courses" table
        builder.ToTable("courses");

        builder.HasKey(course => course.Id);

        // Owner (FK -> users.id, char(36))
        builder.Property(course => course.OwnerUserId).HasColumnType("char(36)").IsRequired();

        // Course fields
        builder.Property(course => course.Title).HasMaxLength(180).IsRequired();
        builder.Property(course => course.Description).HasColumnType("text").IsRequired();
        builder.Property(course => course.StartsAtUtc).IsRequired();
        builder.Property(course => course.EndsAtUtc).IsRequired();
        builder.Property(course => course.Location).HasMaxLength(180).IsRequired();
        
        // Capacity must be >= 1; Status enum stored as int
        builder.Property(course => course.Capacity).IsRequired();
        builder.Property(course => course.Status).IsRequired();

        // Audit timestamps (UTC)
        builder.Property(course => course.CreatedAtUtc).IsRequired();
        builder.Property(course => course.UpdatedAtUtc).IsRequired();
    }
}