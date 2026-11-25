# Kursplassen

REST API for course publishing and participant registration, with authentication, course ownership and self-service registrations.

## Table of contents

- [Project status](#project-status)
- [Technology stack](#technology-stack)
- [Architecture](#architecture)
- [Project structure](#project-structure)
- [Getting started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [1. Restore tools and dependencies](#1-restore-tools-and-dependencies)
  - [2. Start MySQL](#2-start-mysql)
  - [3. Check database configuration](#3-check-database-configuration)
  - [4. Apply database migrations](#4-apply-database-migrations)
  - [5. Run the API](#5-run-the-api)
- [Testing](#testing)
- [Database](#database)
- [Supporting files](#supporting-files)
- [AI usage](#ai-usage)

## Project status

**In progress.**

Implemented so far:

- **HTTP surface (Minimal API)**: health check, root landing page, auth endpoints (`/api/auth/register`, `/api/auth/login`, `/api/auth/token`, `/api/auth/token/refresh`), course endpoints (`GET`/`POST /api/courses`, `GET`/`PUT`/`DELETE /api/courses/{courseId}`), registration endpoints (`POST /api/courses/{courseId}/registrations`, `GET /api/registrations/mine`, `DELETE /api/registrations/{registrationId}`).
- **Cross-cutting concerns**: structured request logging, global exception handling (RFC 7807 `ProblemDetails`), rate limiting (fixed window, 100 req/min per partition, `429` on rejection), dev-only Swagger UI + Scalar, health check.
- **Domain**: entities, DTOs, services and exceptions aligned with the assignment ER diagram; auth (BCrypt hashing, JWT), course and registration business logic.
- **Data**: EF Core + Pomelo MySQL, code-first, migration applied.

Still pending: automated tests, demo data seeding, final documentation, video, submission packaging.

## Technology stack

- ASP.NET Core 10 (Minimal API)
- Entity Framework Core 9 + Pomelo.EntityFrameworkCore.MySql (MySQL 8)
- MySQL 8
- xUnit, Moq, WebApplicationFactory (integration tests); SQLite for isolated integration tests
- FluentValidation
- ASP.NET Core rate limiting (built-in)
- JWT bearer (`Microsoft.AspNetCore.Authentication.JwtBearer` + IdentityModel) + BCrypt.Net
- Scalar.AspNetCore (OpenAPI UI in development)

## Architecture

The solution follows a layered structure:

- **`Kursplassen.Api`** – presentation layer: endpoints, middleware, authentication, composition root.
- **`Kursplassen.Core`** – domain layer: entities, enums, DTOs, services, interfaces, domain exceptions (no infrastructure dependencies).
- **`Kursplassen.Infrastructure`** – data layer: EF Core `DbContext`, configurations, repositories.

Request flow: Minimal API endpoint → service (business rules) → repository (EF Core) → MySQL.

## Project structure

```text
Kursplassen/                                            # Solution root
├── Kursplassen.sln
├── global.json
├── dotnet-tools.json
├── .gitignore
├── .editorconfig
│
├── src/
│   ├── Kursplassen.Api/                                # Presentation layer
│   │   ├── Endpoints/                                  # Minimal API endpoint mappings
│   │   ├── Middleware/                                  # Logging, exception handling, rate limiting
│   │   ├── Authentication/                             # JWT options/services, current user
│   │   ├── Extensions/                                 # Composition root (DI)
│   │   ├── Properties/
│   │   ├── appsettings.json / appsettings.Development.json
│   │   ├── Program.cs
│   │   └── Kursplassen.Api.csproj
│   │
│   ├── Kursplassen.Core/                               # Domain layer
│   │   ├── Entities/
│   │   ├── Enums/
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Services/
│   │   └── Exceptions/
│   │
│   └── Kursplassen.Infrastructure/                     # Data layer
│       ├── Data/
│       ├── Configurations/
│       ├── Repositories/
│       ├── Migrations/
│       ├── Seeding/
│       └── Kursplassen.Infrastructure.csproj
│
├── tests/
│   ├── Kursplassen.UnitTests/
│   └── Kursplassen.IntegrationTests/
│
├── docs/
│   ├── Assignment.md
│   ├── Checklist.md
│   ├── design-refleksjon.md
│   └── Endpoint-defenitions.md
│
├── README.md
├── KI-refleksjon.md
├── requests.http
└── video-link.txt
```

## Getting started

### Prerequisites

- .NET 10 SDK
- EF Core tools (`dotnet tool restore`)
- MySQL 8 — or use the provided MySQL container (Docker or Podman):

  ```bash
  podman run -d --name kursplassen-mysql -e MYSQL_ROOT_PASSWORD=changeme -e MYSQL_DATABASE=kursplassen -p 3306:3306 docker.io/library/mysql:8
  ```

### 1. Restore tools and dependencies

```bash
dotnet tool restore
dotnet restore
```

### 2. Start MySQL

Start the MySQL container (or local instance) and wait until it accepts connections.

### 3. Check database configuration

Confirm the connection string `ConnectionStrings:Kursplassen` points to the running database (default `Server=localhost;Database=kursplassen;...`).

### 4. Apply database migrations

```bash
dotnet ef database update --project src/Kursplassen.Infrastructure --startup-project src/Kursplassen.Api
```

### 5. Run the API

```bash
dotnet run --project src/Kursplassen.Api
```

The API is then available at the configured local URL (e.g. `http://localhost:5029`), with:

- `GET /health` – health check
- `GET /` – landing page
- `/swagger` and `/scalar` – API documentation (Development)
- `/api/...` – API endpoints

## Testing

```bash
dotnet test Kursplassen.sln
```

## Database

Code-first with EF Core migrations (Pomelo MySQL provider, MySQL 8).

1. Start MySQL (container or local instance).
2. Apply migrations:

   ```bash
   dotnet ef database update --project src/Kursplassen.Infrastructure --startup-project src/Kursplassen.Api
   ```

## Supporting files

- `README.md` – this file.
- `KI-refleksjon.md` – AI usage reflection.
- `docs/Assignment.md` – exam assignment text.
- `docs/Checklist.md` – progress checklist.
- `docs/design-refleksjon.md` – design reflection.
- `docs/Endpoint-defenitions.md` – endpoint overview.
- `requests.http` – sample HTTP requests.
- `video-link.txt` – demo video link.

## AI usage

See [KI-refleksjon.md](KI-refleksjon.md).
