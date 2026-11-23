using Kursplassen.Core.Enums;

namespace Kursplassen.Core.Entities;

public class AuditLog
{
    public Guid Id { get; set; }

    // Category of the audit entry (e.g. request, error, auth)
    public AuditLogType Type { get; set; }

    // Short event name/key (e.g. "REQUEST_FAILED")
    public string Event { get; set; } = string.Empty;

    // Human-readable description
    public string Message { get; set; } = string.Empty;

    // Optional serialized key/value data (JSON)
    public string? Properties { get; set; }

    // Exception details (only for error entries)
    public string? Exception { get; set; }

    public string? ExceptionType { get; set; }

    public string? StackTrace { get; set; }

    // Correlation identifiers for tracing
    public string? RequestId { get; set; }

    public string? TraceIdentifier { get; set; }

    public string? CorrelationId { get; set; }

    // HTTP request context
    public string? HttpMethod { get; set; }

    public string? Path { get; set; }

    public string? QueryString { get; set; }

    public int? StatusCode { get; set; }

    // Request duration in milliseconds
    public long? DurationMs { get; set; }

    // Acting user context (null for anonymous)
    public string? UserId { get; set; }

    public string? UserEmail { get; set; }

    // Client/connection info
    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    // Timestamp stored as UTC (offset-aware)
    public DateTimeOffset CreatedAtUtc { get; set; }
}