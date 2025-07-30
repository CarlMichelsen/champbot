using System.Threading.Channels;
using Application.Configuration;
using Application.Configuration.Options;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Presentation.DiscordBot;

namespace Application.DiscordBot;

public class DiscordBot(
    ILogger<DiscordBot> logger,
    IOptions<DiscordBotOptions> options,
    Channel<SocketMessage> socketMessageChannel,
    DiscordSocketClient discordSocketClient,
    InteractionService interactionService,
    IServiceProvider rootServiceProvider,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
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
            // Clear existing commands first
            if (options.Value.DebugGuild is not null)
            {
                await discordSocketClient.Rest.GetGuildApplicationCommands(
                    options.Value.DebugGuild.Value);
            }
            
            await interactionService.AddModulesAsync(typeof(DiscordBot).Assembly, rootServiceProvider);
            
            if (options.Value.DebugGuild is not null)
            {
                await interactionService.RegisterCommandsToGuildAsync(options.Value.DebugGuild.Value);
            }
            else
            {
                await interactionService.RegisterCommandsGloballyAsync();
            }
        };
        
        discordSocketClient.MessageReceived += socketMessage =>
        {
            socketMessageChannel.Writer.TryWrite(socketMessage);
            return Task.CompletedTask;
        };

        discordSocketClient.MessageDeleted += async (m, c) =>
        {
            using var scope = serviceScopeFactory.CreateScope();
            var messageDeletionHandler = scope.ServiceProvider.GetRequiredService<IMessageDeletionHandler>();
            await messageDeletionHandler.HandleMessageDeletion(m, c);
        };
        
        discordSocketClient.InteractionCreated += this.HandleInteraction;
    }
    
    private async Task HandleInteraction(SocketInteraction interaction)
    {
        var context = new SocketInteractionContext(discordSocketClient, interaction);
        var result = await interactionService.ExecuteCommandAsync(context, rootServiceProvider);
        if (!result.IsSuccess && result.Error is not null)
        {
            if (result.Error.Value == InteractionCommandError.Exception)
            {
                logger.LogCritical(result.ErrorReason);
            }
        }
    }
}