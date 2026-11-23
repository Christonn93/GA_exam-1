using Kursplassen.Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kursplassen.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Map entity to the "users" table
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        // Email: required, capped at RFC max length
        builder.Property(user => user.Email)
            .HasMaxLength(254)
            .IsRequired();

        // Emails must be unique across users
        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.Property(user => user.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(user => user.DisplayName)
            .HasMaxLength(120)
            .IsRequired();
    }
}