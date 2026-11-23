namespace Kursplassen.Core.Exceptions;

// Thrown when an operation conflicts with existing state (e.g. duplicate email).
// Mapped to HTTP 409 by ExceptionHandlingMiddleware.
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}