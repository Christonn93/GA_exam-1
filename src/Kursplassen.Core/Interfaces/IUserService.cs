using Kursplassen.Core.DTOs.Users;

namespace Kursplassen.Core.Interfaces;

public interface IUserService
{
    // Fetch a user by ID and map it to a public response (throws NotFound when missing)
    Task<UserResponse> GetByIdAsync(Guid userId);
}
