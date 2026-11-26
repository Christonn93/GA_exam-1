using Kursplassen.Core.Entities;
using Kursplassen.Core.Enums;

namespace Kursplassen.Core.Interfaces;

public interface IAuditLogRepository
{
    // List audit log entries, optionally filtered by type, newest first
    Task<IReadOnlyList<AuditLog>> GetAllAsync(AuditLogType? type = null);

    // Fetch a single audit log entry by ID (null when missing)
    Task<AuditLog?> GetByIdAsync(Guid auditLogId);
}
