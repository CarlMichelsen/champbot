using Api;
using Api.Endpoints;
using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Dependencies
builder.RegisterChampBotDependencies();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    // "Yeah, I'm gonna need you to be more groovy than that"
    await app.Services.EnsureLatestDatabaseMigration();
}

// OpenApi and Scalar endpoints - only enabled in development mode
app.MapOpenApiAndScalarForDevelopment();

// Handle uncaught exceptions
app.UseExceptionHandler();

// Application endpoints
app.MapApplicationEndpoints();

// Output cache
app.UseOutputCache();

app.LogStartup();

app.Run();