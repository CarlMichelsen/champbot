using System.Reactive.Linq;
using Discord;
using Discord.WebSocket;
using DiscordBot.Accessor;
using DiscordBot.Configuration;
using Interface.Accessor;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordBot.Service;

public class DiscordBotBackgroundService(
    ILogger<DiscordBotBackgroundService> logger,
    IDiscordSocketClientAccessor discordSocketClientAccessor,
    IOptions<DiscordBotOptions> discordBotOptions) : BackgroundService
{
    private readonly DiscordSocketClient socketClient = new(
        DiscordSocketConfigFactory.GetConfig());

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // Make discord bot available to IDiscordSocketClientAccessor
            ((DiscordSocketClientAccessor)discordSocketClientAccessor)
                .SetDiscordSocketClient(this.socketClient);

            this.socketClient.Log += this.LogAsync;

            await this.socketClient.LoginAsync(
                TokenType.Bot,
                discordBotOptions.Value.Token);

            await this.socketClient.StartAsync();

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            logger.LogInformation(
                "DiscordBot is aware of the following guilds:\n{Guilds}",
                string.Join('\n', this.socketClient.Guilds.Select(g => $"\t{g.Name}:{g.Id}")));
            
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (TaskCanceledException)
        {
            await this.socketClient.DisposeAsync();
            logger.LogInformation("Discord bot was stopped safely");
        }
        catch (System.Exception e)
        {
            logger.LogCritical(e, "Discord bot failed to start");
        }
    }

    private Task LogAsync(LogMessage log)
    {
        logger.LogInformation("DiscordBot {LogMessage}", log.ToString());
        return Task.CompletedTask;
    }
}