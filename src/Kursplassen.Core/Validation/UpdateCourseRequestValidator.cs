using FluentValidation;
using Kursplassen.Core.DTOs.Courses;

namespace Kursplassen.Core.Validation;

public class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseRequest>
{
    public UpdateCourseRequestValidator()
    {
        // Title: required, max 180 chars (matches DB column length)
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(180).WithMessage("Title must not exceed 180 characters.");

        // Description: required free text
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        // Location: required, max 180 chars (matches DB column length)
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(180).WithMessage("Location must not exceed 180 characters.");

        // Capacity: must allow at least one participant
        RuleFor(x => x.Capacity)
            .GreaterThanOrEqualTo(1).WithMessage("Capacity must be at least 1.");

        // Start time: required
        RuleFor(x => x.StartsAtUtc)
            .NotEmpty().WithMessage("Start time is required.");

        // End time: required and must come after the start time
        RuleFor(x => x.EndsAtUtc)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(x => x.StartsAtUtc).WithMessage("End time must be after start time.");

        // Status: must map to a defined CourseStatus value
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid course status.");
    }
}