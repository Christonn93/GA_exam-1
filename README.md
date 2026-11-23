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


## Project structure

```
Kursplassen/                                            # Solution root
├── Kursplassen.sln                                     # Visual Studio solution file
├── global.json                                         # Pins the .NET SDK version
├── dotnet-tools.json                                   # Local dotnet tool manifest (e.g. EF CLI)
├── .gitignore                                          # Git ignore rules
├── .editorconfig                                       # Code style / formatting rules
│
├── src/                                                # Production code
│   ├── Kursplassen.Api/                                # Presentation layer (HTTP surface)
│   │   ├── Controllers/                                # API controllers
│   │   │   ├── AuthController.cs                       # Register / login endpoints
│   │   │   ├── CoursesController.cs                    # Course CRUD endpoints
│   │   │   └── RegistrationsController.cs              # Course registration endpoints
│   │   ├── Middleware/
│   │   │   └── ExceptionHandlingMiddleware.cs          # Maps exceptions to ProblemDetails
│   │   ├── Authentication/
│   │   │   ├── JwtOptions.cs                           # JWT config (issuer, key, lifetime)
│   │   │   ├── JwtTokenService.cs                      # Issues JWT tokens
│   │   │   └── CurrentUserService.cs                   # Reads current user from HttpContext
│   │   ├── Extensions/
│   │   │   └── DependencyInjection.cs                  # Composition root / service registration
│   │   ├── Properties/
│   │   │   └── launchSettings.json                     # Local run profiles
│   │   ├── appsettings.json                            # Base configuration
│   │   ├── appsettings.Development.json                # Dev overrides
│   │   ├── Program.cs                                  # App entry point / pipeline setup
│   │   └── Kursplassen.Api.csproj
│   │
│   ├── Kursplassen.Core/                               # Domain layer (no infra dependencies)
│   │   ├── Entities/
│   │   │   ├── User.cs                                 # User aggregate root
│   │   │   ├── Course.cs                               # Course aggregate root
│   │   │   └── Registration.cs                         # User <-> Course link
│   │   ├── Enums/
│   │   │   ├── UserRole.cs                             # e.g. Admin / Instructor / Student
│   │   │   └── CourseStatus.cs                         # e.g. Draft / Published / Archived
│   │   ├── DTOs/                                       # Request/response contracts
│   │   │   ├── Auth/                                   # RegisterRequest, LoginRequest, LoginResponse
│   │   │   ├── Courses/                                # Create/Update/Response DTOs
│   │   │   └── Registrations/                          # RegistrationResponse
│   │   ├── Interfaces/                                 # Abstractions (ports)
│   │   │   ├── IUserRepository.cs                      # Repositories
│   │   │   ├── ICourseRepository.cs
│   │   │   ├── IRegistrationRepository.cs
│   │   │   ├── IAuthService.cs                         # Services
│   │   │   ├── ICourseService.cs
│   │   │   ├── IRegistrationService.cs
│   │   │   ├── IJwtTokenService.cs
│   │   │   └── ICurrentUserService.cs
│   │   ├── Services/                                   # Business logic implementations
│   │   │   ├── AuthService.cs
│   │   │   ├── CourseService.cs
│   │   │   └── RegistrationService.cs
│   │   ├── Exceptions/                                 # Domain-level exceptions
│   │   │   ├── NotFoundException.cs
│   │   │   ├── ForbiddenException.cs
│   │   │   └── ConflictException.cs
│   │   └── Kursplassen.Core.csproj
│   │
│   └── Kursplassen.Infrastructure/                     # Data / external concerns
│       ├── Data/
│       │   └── KursplassenDbContext.cs                 # EF Core DbContext
│       ├── Configurations/                             # IEntityTypeConfiguration<T> classes
│       │   ├── UserConfiguration.cs
│       │   ├── CourseConfiguration.cs
│       │   └── RegistrationConfiguration.cs
│       ├── Repositories/                               # EF Core repository implementations
│       │   ├── UserRepository.cs
│       │   ├── CourseRepository.cs
│       │   └── RegistrationRepository.cs
│       ├── Migrations/                                 # EF Core migrations
│       ├── Seeding/
│       │   └── DatabaseSeeder.cs                       # Seeds initial/test data
│       └── Kursplassen.Infrastructure.csproj
│
├── tests/                                              # Test projects
│   ├── Kursplassen.UnitTests/                          # Fast, isolated tests
│   │   ├── Services/
│   │   │   ├── CourseServiceTests.cs
│   │   │   └── RegistrationServiceTests.cs
│   │   ├── Validation/
│   │   │   └── CourseValidationTests.cs
│   │   └── Kursplassen.UnitTests.csproj
│   │
│   └── Kursplassen.IntegrationTests/                   # End-to-end HTTP tests
│       ├── Fixtures/
│       │   └── KursplassenWebApplicationFactory.cs     # Spins up real app
│       ├── Endpoints/
│       │   ├── AuthEndpointTests.cs
│       │   ├── CourseEndpointTests.cs
│       │   └── RegistrationEndpointTests.cs
│       └── Kursplassen.IntegrationTests.csproj
│
├── README.md                                           # Project overview / setup instructions
├── KI-refleksjon.md                                    # AI-use reflection document
├── requests.http                                       # Sample HTTP requests for manual testing
└── video-link.txt                                      # Link to demo video
```



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
