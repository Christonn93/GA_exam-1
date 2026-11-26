using Kursplassen.Core.Enums;

namespace Kursplassen.Core.DTOs.Audit;

public class AuditLogResponse
{
    public Guid Id { get; set; }

    public string Type { get; set; } = default!; // e.g. "Information" / "Warning" / "Error" / "Security"

    public string Event { get; set; } = default!; // short event name / key

    public string Message { get; set; } = default!; // human-readable description

    public string? Properties { get; set; } // optional serialized key/value data (JSON)

    public string? Exception { get; set; }

    public string? ExceptionType { get; set; }

    public string? StackTrace { get; set; }

    public string? RequestId { get; set; }

    public string? TraceIdentifier { get; set; }

    public string? CorrelationId { get; set; }

    public string? HttpMethod { get; set; }

    public string? Path { get; set; }

    public string? QueryString { get; set; }

    public int? StatusCode { get; set; }

    public long? DurationMs { get; set; }

    public string? UserId { get; set; }

    public string? UserEmail { get; set; }

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
}
