namespace Kursplassen.Core.Exceptions;

// Thrown when the caller is authenticated but not allowed to perform the action.
// Mapped to HTTP 403 by ExceptionHandlingMiddleware.
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }
}