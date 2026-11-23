using Kursplassen.Core.Entities;
using Kursplassen.Core.Interfaces;
using Kursplassen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kursplassen.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    // EF Core context used for all user queries and writes
    protected readonly KursplassenDbContext _context;

    public UserRepository(KursplassenDbContext context)
    {
        _context = context;
    }

    // Look up a user by email (used during login/registration);
    // returns null when no account matches
    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users
            .FirstOrDefaultAsync(user => user.Email == email);

    // Load a single user by ID; returns null when no matching row exists
    public async Task<User?> GetByIdAsync(Guid userId)
        => await _context.Users
            .FirstOrDefaultAsync(user => user.Id == userId);

    // Track and persist a new user, then return it so callers
    // can read DB-generated values (e.g. Id)
    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}