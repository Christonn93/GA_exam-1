namespace Kursplassen.Api.Authentication;

public class JwtOptions
{
    // Symmetric signing key used to sign and validate JWTs
    // (must be long/random enough; never commit real values)
    // Dev value is supplied via configuration (appsettings.Development.json / user secrets).
    public string SecretKey { get; set; } = string.Empty;

    // Expected "iss" claim — tokens from other issuers are rejected
    public string ValidIssuer { get; set; } = string.Empty;

    // Expected "aud" claim — tokens for other audiences are rejected
    public string ValidAudience { get; set; } = string.Empty;

    // Lifetime of issued tokens in minutes (default 60)
    public int TokenExpiryMinutes { get; set; } = 60;
}