using Kursplassen.Api.Endpoints;
using Kursplassen.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register application and API-level services
builder.Services
    .AddApplicationServices()
    .AddApiServices();

var app = builder.Build();

// Configure the HTTP pipeline (OpenAPI in dev, logging, authorization)
app.UseApiPipeline();

// Map endpoints
app.MapHealthEndpoints();
app.MapAuthEndpoints();

// Map controllers
app.MapControllers();

app.Run();

// Exposes Program to the test project via WebApplicationFactory<Program>
public partial class Program { }