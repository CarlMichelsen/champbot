using App;
using App.Middleware;
using Database;
using Domain.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterApplicationDependencies();

var app = builder.Build();

app.UseMiddleware<UnhandledExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(ApplicationConstants.DevelopmentCorsPolicyName);
}
else
{
    // "Genius may have its limitations, but stupidity is not thus handicapped." - Elbert Hubbard
    await app.Services
        .EnsureLatestDatabaseMigrationsPushed();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles(StaticFileOptionsFactory.Create());

app.MapFallbackToFile("index.html");

app.RegisterEndpoints();

app.MapGet("health", () => Results.Ok());

app.Services.GetRequiredService<ILogger<Program>>()
    .LogInformation(
        "{ApplicationName} service has started",
        ApplicationConstants.ApplicationName);

app.Run();