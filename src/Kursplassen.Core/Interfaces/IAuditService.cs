using Kursplassen.Core.DTOs.Audit;
using Kursplassen.Core.Enums;

namespace Kursplassen.Core.Interfaces;

public interface IAuditService
{
    // List audit log entries, optionally filtered by type, newest first
    Task<IReadOnlyList<AuditLogResponse>> GetAllAsync(AuditLogType? type = null);

    // Fetch a single audit log entry by ID (throws NotFound when missing)
    Task<AuditLogResponse> GetByIdAsync(Guid auditLogId);
}
