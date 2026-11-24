# Design reflections and improvements

This document captures design choices, trade-offs, and reflections on the
assignment itself — including places where a real-world approach would differ
from the constraints imposed by the exam prompt. It is meant to be read together
with `README.md` and `KI-refleksjon.md`.

---

## Role assignment during registration

### What the assignment requires

The endpoint contract states:

> `POST /api/auth/register` | None | Creates a participant account; the role is
> assigned by the system.

Section 5 makes this explicit:

- The role is set to `Participant` on the server side.
- The client must not be able to choose `Instructor` through the registration
  request.

This is reflected in the implementation: `RegisterRequest` has no role field,
and `AuthService.RegisterAsync` always creates a `UserRole.Participant`.

### The observed limitation

All seed email addresses in the course proposal use the `@example.com` domain.
Because public registration can only ever produce `Participant` accounts, the
only way an `Instructor` account can come into existence is through data
seeding. From a purely functional standpoint this feels limiting: the system
offers no self-service path to instructor accounts.

### A considered alternative

One alternative would be to infer the role from the email domain at registration
time — for example:

- `ida@kurs.instructor.com` → assigned `Instructor`
- any other domain (`@email.com`, `@google.com`, …) → assigned `Participant`

This would make the "system assigns the role" while still allowing instructor
accounts to be created through the public endpoint.

### Why the alternative was rejected

Two reasons:

1. **The exam prompt forbids it.** The instructions explicitly state that
   registration creates participant accounts and that the client cannot choose
   the instructor role. A domain-based rule would not comply with the stated
   structure.

2. **A domain-based rule is still client-controlled input.** The email address
   is supplied by the caller, so a domain heuristic is effectively a role field
   in disguise — anyone could register with a `@kurs.instructor.com` address and
   self-promote. This violates the deeper requirement that *"identity and role
   are taken from the authenticated context, not trusted from the request
   body."*

### The better real-world design

In a production system the answer is not self-service role selection but an
out-of-band provisioning path: an administrator (or an invite flow) creates and
promotes instructor accounts. The exam's "role is assigned by the system"
therefore models a legitimate, security-correct pattern — the ergonomic gap is
that the exam scenario has no admin/invite mechanism, so instructors are
materialized through seed data instead.

### Decision

Keep public registration participant-only, per the assignment. Instructor
accounts are created through the `DatabaseSeeder` (using the seeded
`ida@example.com` / `omar@example.com` identities) so the course-ownership flows
can still be demonstrated.

---

