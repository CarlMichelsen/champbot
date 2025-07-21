using Application.Configuration.Options;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.DiscordBot;

public class DiscordBot(
    ILogger<DiscordBot> logger,
    IOptions<DiscordBotOptions> options,
    DiscordSocketClient discordSocketClient,
    InteractionService interactionService,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Discord bot starting...");
            
            this.ConfigureDiscordSocketClient();
            await discordSocketClient.LoginAsync(
                TokenType.Bot,
                options.Value.Token);
            await discordSocketClient.StartAsync();

            // Keep running until cancellation is requested
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            // Expected when cancellation is requested
        }
    }

    private void ConfigureDiscordSocketClient()
    {
        discordSocketClient.Log += msg =>
        {
            logger.Log(msg);
            return Task.CompletedTask;
        };
        discordSocketClient.Ready += async () =>
        {
            // Be more specific about which assembly contains your modules
            await interactionService.AddModulesAsync(typeof(DiscordBot).Assembly, serviceProvider);
            
            if (options.Value.DebugGuild is not null)
            {
                await interactionService.RegisterCommandsToGuildAsync(options.Value.DebugGuild.Value);
            }
            else
            {
                await interactionService.RegisterCommandsGloballyAsync();
            }
        };
        discordSocketClient.InteractionCreated += this.HandleInteraction;
    }
    
    private async Task HandleInteraction(SocketInteraction interaction)
    {
        var context = new SocketInteractionContext(discordSocketClient, interaction);
        await interactionService.ExecuteCommandAsync(context, serviceProvider);
    }
}