using Kursplassen.Api.Authentication;
using Kursplassen.Api.Endpoints;
using Kursplassen.Api.Extensions;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Keep the console output focused: suppress verbose EF Core SQL-command
// logging while keeping application-level (middleware/service) logs.
builder.Services.AddLogging(logging =>
{
    logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
});

// Register application, API, and authentication services in order
builder.Services
    .AddApplicationServices(builder.Configuration)   // domain services + DbContext
    .AddApiServices()                                 // controllers + OpenAPI
    .AddApiAuthentication(builder.Configuration);    // JWT + cookie auth

var app = builder.Build();

// Configure the HTTP pipeline (OpenAPI in dev, logging, authorization)
app.UseApiPipeline();

// Map endpoints
app.MapHealthEndpoints();
app.MapRootEndpoints();
app.MapAuthEndpoints();
app.MapCoursesEndpoints();
app.MapRegistrationsEndpoints();
app.MapUserEndpoints();
app.MapAuditEndpoints();

// Map controllers
app.MapControllers();

app.Run();

// Exposes Program to the test project via WebApplicationFactory<Program>
public partial class Program { }