using Kursplassen.Core.Entities;
using Kursplassen.Core.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Kursplassen.Api.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _options;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public Task<string> GenerateTokenAsync(User user)
    {
        // Build the symmetric signing key from the configured secret
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_options.SecretKey));

        // Sign with HMAC-SHA256 (must match the validation config)
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Standard claims consumed by [Authorize] and User.Identity
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // user id
            new Claim(ClaimTypes.Name, user.DisplayName),             // display name
            new Claim(ClaimTypes.Role, user.Role.ToString())          // role for [Authorize(Roles=...)]
        };

        var token = new JwtSecurityToken(
            issuer: _options.ValidIssuer,
            audience: _options.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.TokenExpiryMinutes),
            signingCredentials: credentials);

        // Serialize to the compact JWT string sent to the client
        var tokenHandler = new JwtSecurityTokenHandler();
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}