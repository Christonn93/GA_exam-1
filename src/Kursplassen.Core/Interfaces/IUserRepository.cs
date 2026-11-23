using Kursplassen.Core.Entities;

namespace Kursplassen.Core.Interfaces;

public interface IUserRepository
{
    // Look up a user by email (null if not found)
    Task<User?> GetByEmailAsync(string email);

    // Fetch a single user by ID (null if not found)
    Task<User?> GetByIdAsync(Guid userId);

    // Persist a new user and return the tracked entity
    Task<User> AddAsync(User user);
}