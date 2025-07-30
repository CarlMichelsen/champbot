using System.Threading.Channels;
using Api.Extensions;
using Api.HostedServices;
using Application.Client.Discord;
using Application.Configuration.Options;
using Application.DiscordBot;
using Database;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Presentation.Client.Discord;
using Presentation.DiscordBot;

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
        builder.Services
            .AddScoped<IMessageDeletionHandler, MessageDeletionHandler>()
            .AddScoped<IDiscordMessageCleanupHandler, DiscordMessageCleanupHandler>()
            .AddHostedService<MessageCleanupJob>()
            .AddScoped<ISocketMessageHandler, SocketMessageHandler>()
            .AddSingleton<Channel<SocketMessage>>(
                _ => Channel.CreateUnbounded<SocketMessage>(new UnboundedChannelOptions
                {
                    SingleReader = true,
                    AllowSynchronousContinuations = false,
                }))
            .AddHostedService<MessageProcessor>();
        
        // Database
        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b =>
                    {
                        b.MigrationsAssembly("Api");
                        b.MigrationsHistoryTable("__EFMigrationsHistory", DatabaseContext.SchemaName);
                    })
                .UseSnakeCaseNamingConvention();
            
            if (builder.Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });
        
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