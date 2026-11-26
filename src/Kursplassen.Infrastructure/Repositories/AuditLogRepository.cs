using Kursplassen.Core.Entities;
using Kursplassen.Core.Enums;
using Kursplassen.Core.Interfaces;
using Kursplassen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kursplassen.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    // EF Core context used for all audit-log queries and writes
    protected readonly KursplassenDbContext _context;

    public AuditLogRepository(KursplassenDbContext context)
    {
        _context = context;
    }

    // List audit entries, optionally filtered by type, newest first
    public async Task<IReadOnlyList<AuditLog>> GetAllAsync(AuditLogType? type = null)
    {
        var query = _context.AuditLogs.AsQueryable();

        if (type is not null)
        {
            query = query.Where(entry => entry.Type == type);
        }

        return await query
            .OrderByDescending(entry => entry.CreatedAtUtc)
            .ToListAsync();
    }

    // Load a single audit entry by ID; returns null when no matching row exists
    public async Task<AuditLog?> GetByIdAsync(Guid auditLogId)
        => await _context.AuditLogs
            .FirstOrDefaultAsync(entry => entry.Id == auditLogId);
}
