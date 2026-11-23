namespace Kursplassen.Core.Exceptions;

public class ValidationException : Exception
{
    // Field-level validation errors keyed by property name
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    // Message-only constructor — used when no per-field errors are available
    public ValidationException(string message)
        : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    // Full constructor — carries per-field errors for a 400 response body
    public ValidationException(string message, IReadOnlyDictionary<string, string[]> errors)
        : base(message)
    {
        Errors = errors;
    }
}