using Kursplassen.Core.DTOs.Registrations;
using Kursplassen.Core.Entities;
using Kursplassen.Core.Enums;
using Kursplassen.Core.Exceptions;
using Kursplassen.Core.Interfaces;

namespace Kursplassen.Core.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly ICourseRepository _courseRepository;

    public RegistrationService(
        IRegistrationRepository registrationRepository,
        ICourseRepository courseRepository)
    {
        _registrationRepository = registrationRepository;
        _courseRepository = courseRepository;
    }

    public async Task<RegistrationResponse> RegisterAsync(Guid courseId, Guid participantUserId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId)
            ?? throw new NotFoundException("Course not found.");

        // Only published courses that haven't started yet accept registrations
        if (course.Status != CourseStatus.Published)
        {
            throw new ConflictException("Only published courses can accept registrations.");
        }

        if (course.StartsAtUtc <= DateTime.UtcNow)
        {
            throw new ConflictException("Cannot register for a course that has already started.");
        }

        // Capacity must leave room for at least one more spot
        var activeCount = await _registrationRepository.GetActiveCountAsync(courseId);
        if (activeCount >= course.Capacity)
        {
            throw new ConflictException("This course is fully booked.");
        }

        // A participant may hold only one active registration per course
        var duplicate = await _registrationRepository.GetActiveByCourseAndParticipantAsync(courseId, participantUserId);
        if (duplicate is not null)
        {
            throw new ConflictException("You already have an active registration for this course.");
        }

        var registration = new Registration
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            ParticipantUserId = participantUserId,
            Status = RegistrationStatus.Active,
            RegisteredAtUtc = DateTime.UtcNow,
            CancelledAtUtc = null,
            ActiveParticipantUserId = participantUserId,
            Course = course
        };

        var saved = await _registrationRepository.AddAsync(registration);

        return new RegistrationResponse
        {
            Id = saved.Id,
            CourseId = saved.CourseId,
            CourseTitle = course.Title,
            ParticipantUserId = saved.ParticipantUserId,
            Status = saved.Status.ToString(),
            RegisteredAtUtc = saved.RegisteredAtUtc,
            CancelledAtUtc = saved.CancelledAtUtc
        };
    }

    public async Task<IReadOnlyList<RegistrationResponse>> GetMineAsync(Guid participantUserId)
    {
        var registrations = await _registrationRepository.GetByParticipantAsync(participantUserId);
        var responses = new List<RegistrationResponse>();

        foreach (var registration in registrations)
        {
            var course = await _courseRepository.GetByIdAsync(registration.CourseId);
            responses.Add(new RegistrationResponse
            {
                Id = registration.Id,
                CourseId = registration.CourseId,
                CourseTitle = course?.Title ?? "Unknown course",
                ParticipantUserId = registration.ParticipantUserId,
                Status = registration.Status.ToString(),
                RegisteredAtUtc = registration.RegisteredAtUtc,
                CancelledAtUtc = registration.CancelledAtUtc
            });
        }

        return responses;
    }

    public async Task CancelAsync(Guid registrationId, Guid participantUserId)
    {
        var registration = await _registrationRepository.GetByIdAsync(registrationId)
            ?? throw new NotFoundException("Registration not found.");

        // Only the owning participant may cancel
        if (registration.ParticipantUserId != participantUserId)
        {
            throw new ForbiddenException("You can only cancel your own registration.");
        }

        // Idempotent: already cancelled -> nothing to do
        if (registration.Status == RegistrationStatus.Cancelled)
        {
            return;
        }

        registration.Status = RegistrationStatus.Cancelled;
        registration.CancelledAtUtc = DateTime.UtcNow;
        registration.ActiveParticipantUserId = null;

        await _registrationRepository.UpdateAsync(registration);
    }
}
