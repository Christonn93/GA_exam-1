using System.Net;
using Kursplassen.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Kursplassen.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    // Next middleware in the pipeline
    private readonly RequestDelegate _next;

    // Logger for both handled and unhandled exceptions
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            // If the response has already started, we cannot rewrite it.
            // Rethrowing lets the server handle it (connection will abort).
            if (context.Response.HasStarted)
            {
                throw;
            }

            // Translate the domain exception into an HTTP status + title
            var (statusCode, title) = MapException(exception);

            // 5xx = real bug -> log with full exception + stack trace
            if (statusCode == (int)HttpStatusCode.InternalServerError)
            {
                _logger.LogError(
                    exception,
                    "Unhandled exception for {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);
            }
            else
            {
                // 4xx = expected domain error -> warning, no stack trace needed
                _logger.LogWarning(
                    "Handled exception ({StatusCode}) for {Method} {Path}: {Message}",
                    statusCode,
                    context.Request.Method,
                    context.Request.Path,
                    exception.Message);
            }

            // Reset any partial response and set the chosen status code
            context.Response.Clear();
            context.Response.StatusCode = statusCode;

            // RFC 7807 ProblemDetails response body
            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,

                // Hide internal error details on 5xx; expose domain messages on 4xx
                Detail = statusCode == (int)HttpStatusCode.InternalServerError
                    ? "An unexpected error occurred."
                    : exception.Message
            };

            // Include per-field validation errors so clients can show them
            if (exception is ValidationException validationException)
            {
                problem.Extensions["errors"] = validationException.Errors;
            }

            await context.Response.WriteAsJsonAsync(problem);
        }
    }

    // Central mapping from domain exceptions to HTTP status codes
    private static (int StatusCode, string Title) MapException(Exception exception) => exception switch
    {
        ValidationException => ((int)HttpStatusCode.BadRequest, "Bad Request"),
        BadHttpRequestException => ((int)HttpStatusCode.BadRequest, "Bad Request"),
        UnauthorizedException => ((int)HttpStatusCode.Unauthorized, "Unauthorized"),
        ForbiddenException => ((int)HttpStatusCode.Forbidden, "Forbidden"),
        NotFoundException => ((int)HttpStatusCode.NotFound, "Not Found"),
        ConflictException => ((int)HttpStatusCode.Conflict, "Conflict"),

        // Any unhandled exception falls back to 500
        _ => ((int)HttpStatusCode.InternalServerError, "Internal Server Error")
    };
}