namespace Kursplassen.Core.Enums;

public enum AuditLogType
{
    Information = 0,
    Warning = 1,
    Error = 2,
    Security = 3,
    RequestFailed = 4,
    UnhandledException = 5
}