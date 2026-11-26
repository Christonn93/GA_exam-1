using Kursplassen.Core.DTOs.Audit;
using Kursplassen.Core.Entities;
using Kursplassen.Core.Enums;
using Kursplassen.Core.Exceptions;
using Kursplassen.Core.Interfaces;

namespace Kursplassen.Core.Services;

public class AuditService : IAuditService
{
    private readonly IAuditLogRepository _auditLogRepository;

    public AuditService(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    public async Task<IReadOnlyList<AuditLogResponse>> GetAllAsync(AuditLogType? type = null)
    {
        // Optional type filter is applied by the repository
        var logs = await _auditLogRepository.GetAllAsync(type);
        return logs.Select(MapToResponse).ToList();
    }

    public async Task<AuditLogResponse> GetByIdAsync(Guid auditLogId)
    {
        // Missing entry -> 404 via NotFoundException
        var log = await _auditLogRepository.GetByIdAsync(auditLogId)
            ?? throw new NotFoundException("Audit log entry not found.");

        return MapToResponse(log);
    }

    // Shared projection from the audit entity to the response contract
    private static AuditLogResponse MapToResponse(AuditLog log) => new()
    {
        Id = log.Id,

        // Enum exposed as a string so clients see "Request"/"Error" etc.
        Type = log.Type.ToString(),

        Event = log.Event,
        Message = log.Message,
        Properties = log.Properties,
        Exception = log.Exception,
        ExceptionType = log.ExceptionType,
        StackTrace = log.StackTrace,
        RequestId = log.RequestId,
        TraceIdentifier = log.TraceIdentifier,
        CorrelationId = log.CorrelationId,
        HttpMethod = log.HttpMethod,
        Path = log.Path,
        QueryString = log.QueryString,
        StatusCode = log.StatusCode,
        DurationMs = log.DurationMs,
        UserId = log.UserId,
        UserEmail = log.UserEmail,
        IpAddress = log.IpAddress,
        UserAgent = log.UserAgent,
        CreatedAtUtc = log.CreatedAtUtc
    };
}