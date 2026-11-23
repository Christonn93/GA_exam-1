# Core setup.

To get a clean and nice structure to get a good start on the exam I decided to ask perplexity to assist me by creating a console command to set it up. 


## ❓ Question: 

I would like a terminal command to install the base.
I want a structure like ths:
```
Kursplassen.sln
├── src/
│   ├── Kursplassen.Api/          (controllers, middleware, Program.cs)
│   ├── Kursplassen.Core/         (entities, DTOs, interfaces, services)
│   └── Kursplassen.Infrastructure/ (EF Core DbContext, repositories, migrations)
├── tests/
│   ├── Kursplassen.UnitTests/    (xUnit + Moq)
│   └── Kursplassen.IntegrationTests/ (WebApplicationFactory + in-memory/SQLite)
├── README.md
├── KI-refleksjon.md
├── requests.http
└── video-link.txt
```

I'm using .net 10

***I included the pdf of the assignemnt***

---

## 💡 Answer: 

Run the following in a **Bash terminal** from the directory where you want to create `Kursplassen`. It scaffolds your five projects, adds project references and base packages, creates the folders and documentation placeholders, and builds the solution.

All projects target **.NET 10**. The script explicitly requests `.sln` because .NET 10 otherwise defaults to `.slnx`. [learn.microsoft](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-sln)

### Database package choice

This setup uses Pomelo 9 with EF Core 9, while targeting `net10.0`. That combination is supported; Pomelo 9 must not be mixed with EF Core 10. The EF tooling and SQLite test provider therefore also use version `9.0.*`. [github](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/issues/2016)

## Copy-paste setup

Requires the .NET 10 SDK to already be installed. Run this in a new location—not inside an existing `Kursplassen` project.

```bash
bash <<'BASH'
set -euo pipefail

# Create the solution root.
mkdir Kursplassen
cd Kursplassen

# Pin the installed SDK, allowing later SDK feature bands in the same major.
dotnet new globaljson \
  --sdk-version "$(dotnet --version)" \
  --roll-forward latestFeature

dotnet new sln --name Kursplassen --format sln
dotnet new gitignore
dotnet new editorconfig
dotnet new tool-manifest

# Create projects.
dotnet new webapi \
  --name Kursplassen.Api \
  --output src/Kursplassen.Api \
  --framework net10.0 \
  --use-controllers

dotnet new classlib \
  --name Kursplassen.Core \
  --output src/Kursplassen.Core \
  --framework net10.0

dotnet new classlib \
  --name Kursplassen.Infrastructure \
  --output src/Kursplassen.Infrastructure \
  --framework net10.0

dotnet new xunit \
  --name Kursplassen.UnitTests \
  --output tests/Kursplassen.UnitTests \
  --framework net10.0

dotnet new xunit \
  --name Kursplassen.IntegrationTests \
  --output tests/Kursplassen.IntegrationTests \
  --framework net10.0

# Add projects to the solution.
dotnet sln Kursplassen.sln add \
  src/Kursplassen.Api/Kursplassen.Api.csproj \
  src/Kursplassen.Core/Kursplassen.Core.csproj \
  src/Kursplassen.Infrastructure/Kursplassen.Infrastructure.csproj \
  tests/Kursplassen.UnitTests/Kursplassen.UnitTests.csproj \
  tests/Kursplassen.IntegrationTests/Kursplassen.IntegrationTests.csproj

# Project dependencies.
dotnet add src/Kursplassen.Api reference \
  src/Kursplassen.Core \
  src/Kursplassen.Infrastructure

dotnet add src/Kursplassen.Infrastructure reference \
  src/Kursplassen.Core

dotnet add tests/Kursplassen.UnitTests reference \
  src/Kursplassen.Core

dotnet add tests/Kursplassen.IntegrationTests reference \
  src/Kursplassen.Api \
  src/Kursplassen.Infrastructure

# API: authentication and password hashing.
dotnet add src/Kursplassen.Api package \
  Microsoft.AspNetCore.Authentication.JwtBearer --version '10.0.*'

dotnet add src/Kursplassen.Api package \
  Microsoft.Extensions.Identity.Core --version '10.0.*'

# EF Core tooling must be available in the startup project.
dotnet add src/Kursplassen.Api package \
  Microsoft.EntityFrameworkCore.Design --version '9.0.*'

# Infrastructure: MySQL provider and EF Core.
dotnet add src/Kursplassen.Infrastructure package \
  Pomelo.EntityFrameworkCore.MySql --version 9.0.0

dotnet add src/Kursplassen.Infrastructure package \
  Microsoft.EntityFrameworkCore --version '9.0.*'

dotnet add src/Kursplassen.Infrastructure package \
  Microsoft.EntityFrameworkCore.Design --version '9.0.*'

# Unit testing.
dotnet add tests/Kursplassen.UnitTests package Moq

# API integration testing and an isolated SQLite database.
dotnet add tests/Kursplassen.IntegrationTests package \
  Microsoft.AspNetCore.Mvc.Testing --version '10.0.*'

dotnet add tests/Kursplassen.IntegrationTests package \
  Microsoft.EntityFrameworkCore.Sqlite --version '9.0.*'

# Local, solution-specific EF CLI.
dotnet tool install dotnet-ef --version '9.0.*'

# Remove generated example classes and tests.
rm -f \
  src/Kursplassen.Core/Class1.cs \
  src/Kursplassen.Infrastructure/Class1.cs \
  src/Kursplassen.Api/WeatherForecast.cs \
  src/Kursplassen.Api/Controllers/WeatherForecastController.cs \
  src/Kursplassen.Api/Kursplassen.Api.http \
  tests/Kursplassen.UnitTests/UnitTest1.cs \
  tests/Kursplassen.IntegrationTests/UnitTest1.cs

# Create application folders.
mkdir -p \
  src/Kursplassen.Api/Controllers \
  src/Kursplassen.Api/Middleware \
  src/Kursplassen.Api/Extensions \
  src/Kursplassen.Api/Authentication \
  src/Kursplassen.Core/Entities \
  src/Kursplassen.Core/Enums \
  src/Kursplassen.Core/DTOs/Auth \
  src/Kursplassen.Core/DTOs/Courses \
  src/Kursplassen.Core/DTOs/Registrations \
  src/Kursplassen.Core/Interfaces \
  src/Kursplassen.Core/Services \
  src/Kursplassen.Core/Exceptions \
  src/Kursplassen.Infrastructure/Data \
  src/Kursplassen.Infrastructure/Configurations \
  src/Kursplassen.Infrastructure/Repositories \
  src/Kursplassen.Infrastructure/Migrations \
  src/Kursplassen.Infrastructure/Seeding \
  tests/Kursplassen.UnitTests/Services \
  tests/Kursplassen.UnitTests/Validation \
  tests/Kursplassen.IntegrationTests/Fixtures \
  tests/Kursplassen.IntegrationTests/Endpoints

# Keep empty scaffold folders in Git.
find src tests -type d -empty -exec touch {}/.gitkeep \;

# Allow WebApplicationFactory<Program> to access the API entry point.
cat >> src/Kursplassen.Api/Program.cs <<'CS'

// Exposes the entry point to the integration test project.
public partial class Program { }
CS

# Initialize development secret storage. No secrets are added here.
dotnet user-secrets init \
  --project src/Kursplassen.Api

cat > README.md <<'MD'
# Kursplassen

REST API for course publishing and participant registrations.

## Technology
- ASP.NET Core 10
- Entity Framework Core 9 and Pomelo 9
- MySQL
- xUnit, Moq and WebApplicationFactory
- SQLite for isolated integration tests

## Prerequisites
- .NET 10 SDK
- MySQL; setup instructions to be completed

## Run
```bash
dotnet tool restore
dotnet restore
dotnet run --project src/Kursplassen.Api
```

## Tests
```bash
dotnet test Kursplassen.sln
```

## Database
Code-first with EF Core migrations.
Database configuration and migration instructions to be completed.

## AI usage
See KI-refleksjon.md.

## Status
Initial scaffold only. Business functionality is not yet implemented.
MD

cat > KI-refleksjon.md <<'MD'
# Refleksjon over bruk av KI

## Hvordan KI ble brukt
KI ble brukt til å foreslå kommandoer for prosjektoppsett,
mappestruktur, prosjektreferanser og pakkeinstallasjon.

## Hva KI bidro med
Et forslag til grunnstruktur for løsningen.

## Hvordan KI påvirket løsningen
Fyll inn hvordan forslagene faktisk påvirket arbeidet.

## Egne vurderinger og endringer
Fyll inn hva du selv kontrollerte, endret og videreutviklet.

## Feil, begrensninger og utfordringer
Fyll inn konkrete erfaringer fra arbeidet.
MD

cat > requests.http <<'HTTP'
# Replace the address with the URL printed by dotnet run.
@baseUrl = http://localhost:5000

### Course catalogue — implement this endpoint first
GET {{baseUrl}}/api/courses
Accept: application/json
HTTP

cat > video-link.txt <<'TXT'
Legg inn tilgjengelig videolenke før innlevering.
TXT

# Verify the scaffold.
dotnet restore Kursplassen.sln
dotnet build Kursplassen.sln --no-restore

printf '\nScaffold created successfully in: %s\n' "$PWD"
printf 'Start API: dotnet run --project src/Kursplassen.Api\n'
BASH
```

## Resulting dependency structure

```text
Kursplassen.Api ───────────────► Kursplassen.Core
       │
       └──► Kursplassen.Infrastructure ──► Kursplassen.Core

Kursplassen.UnitTests ─────────► Kursplassen.Core

Kursplassen.IntegrationTests ──► Kursplassen.Api
                └─────────────► Kursplassen.Infrastructure
```

`Core` has no project references to `Api` or `Infrastructure`. The API project is where you will wire your service and repository implementations together.

The integration-test project includes `Microsoft.AspNetCore.Mvc.Testing`, which provides the infrastructure for testing the API through `WebApplicationFactory`. The added `public partial class Program` exposes the entry point to that test project. [learn.microsoft](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0)

## What this does not implement yet

This is the base structure, not a completed exam solution. You still need to implement:

- Entities, relationships, `DbContext`, MySQL configuration, migrations and seed data.
- Controllers, DTOs, services and repositories.
- JWT configuration, password hashing and ownership checks.
- Validation, global error handling and structured logging.
- Health checks, rate limiting and actual tests.

Those features, along with API examples, documentation, KI reflection and the video, are required by your assignment.

The script installs SQLite for isolated integration tests; it does not replace the assignment’s requirement to use MySQL for the application itself.