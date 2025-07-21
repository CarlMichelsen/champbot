using Api.Extensions;
using Application.Client.Discord;
using Application.Configuration.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Presentation.Client.Discord;

namespace Api;

public static class Dependencies
{
    public static void RegisterChampBotDependencies(this WebApplicationBuilder builder)
    {
        // Configuration
        builder.Services
            .AddOpenApi()
            .AddConfiguredOpenTelemetry()
            .AddSingleton(TimeProvider.System);
        builder.Configuration.AddJsonFile(
            "secrets.json",
            optional: builder.Environment.IsDevelopment(),
            reloadOnChange: false);
        builder
            .AddValidatedOptions<ApplicationOptions>()
            .AddValidatedOptions<DiscordBotOptions>();
        builder.ApplicationUseSerilog();
        
        // Middleware
        builder.Services
            .AddHttpContextAccessor()
            .AddCustomProblemDetails();
        
        // Discord Bot
        builder
            .AddDiscordBot();
        
        // HealthChecks
        builder.Services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());
        
        // Cache
        builder.Services
            .AddMemoryCache()
            .AddOutputCache();
        
        // Client
        builder.Services
            .AddHttpClient<IDiscordWebhookMessageClient, DiscordWebhookMessageClient>()
            .AddStandardResilienceHandler();
    }
}