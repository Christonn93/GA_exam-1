using System.Diagnostics;
namespace Kursplassen.Api.Middleware;

public sealed class RequestLoggingMiddleware
{
    // Header used to propagate/trace request correlation IDs
    private const string RequestIdHeaderName = "X-Request-Id";

    // Paths that should not produce request logs (docs, static assets)
    private static readonly HashSet<string> IgnoredPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/openapi",
        "/swagger",
        "/favicon.ico"
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip logging for excluded paths
        if (ShouldSkipLogging(context))
        {
            await _next(context);
            return;
        }

        // Resolve or generate a correlation ID for this request
        var requestId = GetOrCreateRequestId(context);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);

            stopwatch.Stop();

            // Log successful/short-circuited requests
            LogRequest(
                context,
                requestId,
                stopwatch.ElapsedMilliseconds,
                exception: null);
        }
        catch (Exception exception)
        {
            stopwatch.Stop();

            // Log the failure, then rethrow to preserve pipeline behavior
            LogRequest(
                context,
                requestId,
                stopwatch.ElapsedMilliseconds,
                exception);

            throw;
        }
    }

    private static bool ShouldSkipLogging(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        // Match against ignored prefixes (e.g. /swagger, /openapi)
        return IgnoredPaths.Any(ignoredPath =>
            path.StartsWith(ignoredPath, StringComparison.OrdinalIgnoreCase));
    }

    private static string GetOrCreateRequestId(HttpContext context)
    {
        // Reuse incoming request ID if provided by caller
        if (context.Request.Headers.TryGetValue(
                RequestIdHeaderName,
                out var incomingRequestId) &&
            !string.IsNullOrWhiteSpace(incomingRequestId))
        {
            return incomingRequestId.ToString();
        }

        // Otherwise generate a new ID and echo it back in the response
        var requestId = Guid.NewGuid().ToString("N");

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[RequestIdHeaderName] = requestId;
            return Task.CompletedTask;
        });

        return requestId;
    }

    private void LogRequest(
        HttpContext context,
        string requestId,
        long elapsedMilliseconds,
        Exception? exception)
    {
        var method = context.Request.Method;
        var path = context.Request.Path.Value ?? string.Empty;
        var queryString = context.Request.QueryString.Value ?? string.Empty;
        var statusCode = context.Response.StatusCode;
        // Fall back to "anonymous" when no authenticated identity is present
        var user = context.User.Identity?.IsAuthenticated == true
            ? context.User.Identity.Name
            : "anonymous";

        // Unhandled exception -> log as error with stack trace
        if (exception is not null)
        {
            _logger.LogError(
                exception,
                "❌ REQUEST FAILED | {Method} {Path}{QueryString} | Status: {StatusCode} | Duration: {DurationMs} ms | RequestId: {RequestId} | User: {User}",
                method, path, queryString, statusCode, elapsedMilliseconds, requestId, user);

            return;
        }

        // 5xx without exception object (e.g. written by downstream middleware)
        if (statusCode >= 500)
        {
            _logger.LogError(
                "❌ REQUEST FAILED | {Method} {Path}{QueryString} | Status: {StatusCode} | Duration: {DurationMs} ms | RequestId: {RequestId} | User: {User}",
                method, path, queryString, statusCode, elapsedMilliseconds, requestId, user);

            return;
        }

        // 4xx client errors -> warning level
        if (statusCode >= 400)
        {
            _logger.LogWarning(
                "⚠️ REQUEST CLIENT ERROR | {Method} {Path}{QueryString} | Status: {StatusCode} | Duration: {DurationMs} ms | RequestId: {RequestId} | User: {User}",
                method, path, queryString, statusCode, elapsedMilliseconds, requestId, user);

            return;
        }

        // 2xx/3xx -> informational
        _logger.LogInformation(
            "✅ REQUEST | {Method} {Path}{QueryString} | Status: {StatusCode} | Duration: {DurationMs} ms | RequestId: {RequestId} | User: {User}",
            method, path, queryString, statusCode, elapsedMilliseconds, requestId, user);
    }
}