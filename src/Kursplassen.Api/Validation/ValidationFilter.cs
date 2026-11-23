using FluentValidation;
using Kursplassen.Core.Exceptions;

namespace Kursplassen.Api.Validation;

public sealed class ValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        // Inspect each endpoint argument (e.g. request DTOs)
        foreach (var argument in context.Arguments)
        {
            if (argument is null)
            {
                continue;
            }

            // Resolve IValidator<T> for the argument's runtime type
            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

            // No validator registered for this argument -> skip
            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator)
            {
                continue;
            }

            // Run FluentValidation against the argument
            var result = await validator.ValidateAsync(new ValidationContext<object>(argument));

            if (!result.IsValid)
            {
                // Group messages by property name -> { "Email": ["..."], "Title": ["..."] }
                var errors = result.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.ErrorMessage).ToArray());

                // Bubble up as a domain ValidationException (mapped to 400 + errors)
                throw new Kursplassen.Core.Exceptions.ValidationException("Validation failed.", errors);
            }
        }

        // All arguments valid -> continue to the endpoint handler
        return await next(context);
    }
}