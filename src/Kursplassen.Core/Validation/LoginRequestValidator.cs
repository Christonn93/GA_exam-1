using FluentValidation;
using Kursplassen.Core.DTOs.Auth;

namespace Kursplassen.Core.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        // Email: must be provided (format is not validated here —
        // login only checks it against stored accounts)
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.");

        // Password: must be provided
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}