namespace Kursplassen.Core.Exceptions;

// Thrown when authentication fails (e.g. invalid credentials or missing token).
// Mapped to HTTP 401 by ExceptionHandlingMiddleware.
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}