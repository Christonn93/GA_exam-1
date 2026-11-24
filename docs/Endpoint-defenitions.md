# Mandatory endpoints – overview

Follows the table in `docs/Assignment.md` (Task 1 – REST API implementation).

## Required endpoints

| Method | Route                                     | Access            | Description                                                    |
|--------|-------------------------------------------|-------------------|----------------------------------------------------------------|
| POST   | `/api/auth/register`                      | None              | Creates a participant account; the role is set by the system.  |
| POST   | `/api/auth/login`                         | None              | Validates login and returns a JWT.                             |
| GET    | `/api/courses`                            | None              | Lists published, upcoming courses.                             |
| GET    | `/api/courses/{courseId}`                 | None              | Shows course details, number of registrations, and available spots. |
| POST   | `/api/courses`                            | Instructor        | Creates a course; the signed-in instructor becomes the owner.  |
| PUT    | `/api/courses/{courseId}`                 | CourseOwner       | Updates own course.                                            |
| DELETE | `/api/courses/{courseId}`                 | CourseOwner       | Deletes a course with no active registrations.                 |
| POST   | `/api/courses/{courseId}/registrations`   | Participant       | Registers the signed-in participant for the course.            |
| GET    | `/api/registrations/mine`                 | Participant       | Shows own registrations with course information.               |
| DELETE | `/api/registrations/{registrationId}`     | RegistrationOwner | Cancels own registration and frees the spot.                   |
| GET    | `/health`                                 | None              | Reports health status.                                         |

## Extra endpoints

These are added beyond the required set.

| Method | Route                   | Access        | Description                                                       |
|--------|-------------------------|---------------|-------------------------------------------------------------------|
| GET    | `/api/user/{userId}`    | Authenticated | Returns a user's public profile (display name and role).          |
| GET    | `/api/audit`            | Instructor    | Lists audit-log entries.                                          |
| GET    | `/api/audit/{auditId}`  | Instructor    | Returns a single audit-log entry.                                 |

> **Note on access for the extra endpoints:** the user/profile and audit
> endpoints are not required by the assignment. They were initially sketched as
> public (`None`), but exposing them publicly would leak sensitive data — the
> profile endpoint would expose user emails, and the audit log contains request
> details (IPs, paths, user identifiers). They are therefore restricted:
> `/api/user/{userId}` to any authenticated user (returning only the public
> profile, not the email), and `/api/audit` to `Instructor`. In a real system the
> audit log would belong to an `Admin` role, but the assignment only defines
> `Participant` and `Instructor`.

## Access designations

- **None** – public endpoints (no authentication).
- **Authenticated** – any signed-in user, regardless of role.
- **Participant** – signed-in user with the `Participant` role; can view and cancel own registrations, and register for courses.
- **Instructor** – signed-in user with the `Instructor` role; can create and manage courses.
- **CourseOwner** – access designation for an `Instructor` who owns the course in question; not a separate account or JWT role.
- **RegistrationOwner** – access designation for a `Participant` who owns the registration in question; not a separate account or JWT role.

## Status codes and recommended practice

- `200 OK` – success.
- `201 Created` – created resources (e.g., created course / registration).
- `204 No Content` – deletion (e.g., cancellation).
- `400 Bad Request` – input validation.
- `401 Unauthorized` – missing/invalid authentication.
- `403 Forbidden` – authenticated, but not the right ownership/role.
- `404 Not Found` – resource not found.
- `409 Conflict` – e.g., full course, duplicate active registration.
