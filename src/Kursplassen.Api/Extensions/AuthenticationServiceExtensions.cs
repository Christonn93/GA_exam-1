using System.Text;

using Kursplassen.Api.Authentication;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Kursplassen.Api.Extensions;

public static class AuthenticationServiceExtensions
{
    public static IServiceCollection AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind the JWT settings; fail fast if the section is absent
        var jwtOptions = configuration
            .GetSection("JWT")
            .Get<JwtOptions>()
            ?? throw new InvalidOperationException(
                "The JWT configuration section is missing.");

        // Signing key is mandatory — refuse to start without one
        if (string.IsNullOrWhiteSpace(jwtOptions.SecretKey))
        {
            throw new InvalidOperationException(
                "JWT:SecretKey must be configured.");
        }

        services
            // JWT is the default scheme for API calls
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    // Validate issuer, audience, lifetime and signature on every token
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtOptions.ValidIssuer,
                            ValidAudience = jwtOptions.ValidAudience,

                            // Same symmetric key used to sign tokens in JwtTokenService
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                        };
                })
            // Cookie scheme for browser-based auth, configured from CookieSettings
            .AddCookie(
                CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    configuration.Bind("CookieSettings", options);
                });

        services.AddAuthorization();

        return services;
    }
}