using Api;
using Api.Endpoints;
using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Dependencies
builder.RegisterChampBotDependencies();

var app = builder.Build();

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