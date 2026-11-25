using System.Globalization;
using System.Security.Claims;
using System.Threading.RateLimiting;

using Microsoft.AspNetCore.RateLimiting;

namespace Kursplassen.Api.Middleware;

public static class RateLimitingConfiguration
{
    public static void Configure(RateLimiterOptions options)
    {
        // Return 429 instead of the default rejection status
        options.RejectionStatusCode =
            StatusCodes.Status429TooManyRequests;

        // One global limiter; requests are partitioned per user or per IP
        options.GlobalLimiter =
            PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var partitionKey = GetPartitionKey(context);

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: partitionKey,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        // 100 requests per minute per partition, no queueing
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    });
            });

        // Custom response body when a request is rejected
        options.OnRejected = async (context, cancellationToken) =>
        {
            var httpContext = context.HttpContext;

            httpContext.Response.StatusCode =
                StatusCodes.Status429TooManyRequests;

            // Tell the client how long to wait (from limiter metadata)
            if (context.Lease.TryGetMetadata(
                    MetadataName.RetryAfter,
                    out var retryAfter))
            {
                httpContext.Response.Headers.RetryAfter =
                    ((long)Math.Ceiling(retryAfter.TotalSeconds))
                    .ToString(CultureInfo.InvariantCulture);
            }

            // RFC 7807 ProblemDetails body with a trace id for debugging
            await Results.Problem(
                statusCode: StatusCodes.Status429TooManyRequests,
                title: "Too many requests",
                detail: "You have exceeded the request limit. Please try again later.",
                instance: httpContext.Request.Path.Value,
                extensions: new Dictionary<string, object?>
                {
                    ["traceId"] = httpContext.TraceIdentifier
                })
                .ExecuteAsync(httpContext);
        };
    }

    private static string GetPartitionKey(HttpContext context)
    {
        // Authenticated users get their own bucket keyed by user id
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId =
                context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? context.User.FindFirstValue("sub");

            if (!string.IsNullOrWhiteSpace(userId))
            {
                return $"user:{userId}";
            }
        }

        // Anonymous requests fall back to IP; MapToIPv6 normalizes
        // IPv4-mapped addresses so the same client always hits one bucket
        var ipAddress = context.Connection.RemoteIpAddress?
            .MapToIPv6()
            .ToString();

        return $"ip:{ipAddress ?? "unknown"}";
    }
}