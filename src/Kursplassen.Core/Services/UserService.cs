using Kursplassen.Core.DTOs.Users;
using Kursplassen.Core.Entities;
using Kursplassen.Core.Exceptions;
using Kursplassen.Core.Interfaces;

namespace Kursplassen.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> GetByIdAsync(Guid userId)
    {
        // Missing user -> 404 via NotFoundException
        var user = await _userRepository.GetByIdAsync(userId)
                   ?? throw new NotFoundException("User not found.");

        // Public profile only — no email or password hash is exposed
        return new UserResponse
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Role = user.Role.ToString(),
            CreatedAtUtc = user.CreatedAtUtc
        };
    }
}