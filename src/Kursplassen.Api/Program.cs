using Kursplassen.Api.Endpoints;
using Kursplassen.Api.Extensions;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Register application and API-level services
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddApiServices()
    
    // Resource: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-11.0
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => builder.Configuration.Bind("JwtSettings", options))
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => builder.Configuration.Bind("CookieSettings", options))
;

var app = builder.Build();

// Configure the HTTP pipeline (OpenAPI in dev, logging, authorization)
app.UseApiPipeline();

// Map endpoints
app.MapHealthEndpoints();
app.MapRootEndpoints();
app.MapAuthEndpoints();

// Map controllers
app.MapControllers();

app.Run();

// Exposes Program to the test project via WebApplicationFactory<Program>
public partial class Program { }