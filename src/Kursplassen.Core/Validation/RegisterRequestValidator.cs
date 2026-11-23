using FluentValidation;
using Kursplassen.Core.DTOs.Auth;

namespace Kursplassen.Core.Validation;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        // Email: required, must be well-formed, max 254 chars (matches DB column)
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(254).WithMessage("Email must not exceed 254 characters.");

        // Password: required, minimum 8 chars (upper bound not enforced here;
        // BCrypt truncates inputs beyond 72 bytes, worth noting for hardening)
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.");

        // Display name: required, max 120 chars (matches DB column)
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required.")
            .MaximumLength(120).WithMessage("Display name must not exceed 120 characters.");
    }
}