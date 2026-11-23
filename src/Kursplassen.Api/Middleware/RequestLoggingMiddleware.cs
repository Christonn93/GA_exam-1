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

    // Next middleware in the pipeline
    private readonly RequestDelegate _next;

    // Logger scoped to this middleware's category
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

        // Start the timer before invoking the rest of the pipeline
        // so the measured duration covers all downstream work
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);

            // Stop before logging so the duration excludes logging overhead
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
            // Stop the timer as soon as the failure is caught
            stopwatch.Stop();

            // Log the failure, then rethrow to preserve pipeline behavior
            // (the exception-handling middleware above still sees it)
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
        // Using StartsWith so sub-paths like /swagger/index.html are skipped too
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

        // OnStarting fires just before headers are sent, which is the
        // earliest point we can safely add a response header
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
        // Snapshot request/response values once for consistent logging
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