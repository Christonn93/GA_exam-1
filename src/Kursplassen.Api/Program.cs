using Kursplassen.Api.Authentication;
using Kursplassen.Api.Endpoints;
using Kursplassen.Api.Extensions;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Register application and API-level services
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddApiServices()
    
    // Resource: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-11.0
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var jwtOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>() ?? new JwtOptions();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.ValidIssuer,
            ValidAudience = jwtOptions.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => builder.Configuration.Bind("CookieSettings", options))
;

var app = builder.Build();

// Configure the HTTP pipeline (OpenAPI in dev, logging, authorization)
app.UseApiPipeline();

// Map endpoints
app.MapHealthEndpoints();
app.MapRootEndpoints();
app.MapAuthEndpoints();
app.MapCoursesEndpoints();
app.MapRegistrationsEndpoints();

// Map controllers
app.MapControllers();

app.Run();

// Exposes Program to the test project via WebApplicationFactory<Program>
public partial class Program { }