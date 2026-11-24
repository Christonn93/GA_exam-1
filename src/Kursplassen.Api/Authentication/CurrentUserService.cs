using System.Security.Claims;
using Kursplassen.Core.Enums;
using Kursplassen.Core.Interfaces;

namespace Kursplassen.Api.Authentication;

public sealed class CurrentUserService : ICurrentUserService
{
    // Provides access to the current HTTP request (and thus its User principal)
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Shortcut to the authenticated principal (null outside a request)
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public Guid UserId
    {
        get
        {
            // Read the user id claim written by JwtTokenService
            var value = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fall back to Guid.Empty when the claim is missing or malformed
            return Guid.TryParse(value, out var id) ? id : Guid.Empty;
        }
    }

    public UserRole Role
    {
        get
        {
            // Read the role claim written by JwtTokenService
            var value = User?.FindFirstValue(ClaimTypes.Role);

            // Default to the lowest-privilege role if the claim is absent/invalid
            return Enum.TryParse<UserRole>(value, out var role) ? role : UserRole.Participant;
        }
    }
}