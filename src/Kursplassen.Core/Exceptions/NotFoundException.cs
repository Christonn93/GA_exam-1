namespace Kursplassen.Core.Exceptions;

// Thrown when a requested resource doesn't exist.
// Mapped to HTTP 404 by ExceptionHandlingMiddleware.
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}