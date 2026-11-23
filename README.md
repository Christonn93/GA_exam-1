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
