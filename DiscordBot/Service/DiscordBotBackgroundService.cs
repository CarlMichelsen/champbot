using System.Reactive.Linq;
using Discord;
using Discord.WebSocket;
using DiscordBot.Accessor;
using DiscordBot.Configuration;
using DiscordBot.Hubs;
using Domain.Discord;
using Implementation.Util;
using Interface.Accessor;
using Interface.Discord;
using Interface.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordBot.Service;

public class DiscordBotBackgroundService(
    ILogger<DiscordBotBackgroundService> logger,
    IHubContext<VoiceHub, IVoiceClientMethods> hubContext,
    IDiscordBotAccessor discordSocketClientAccessor,
    IOptions<DiscordBotOptions> discordBotOptions) : BackgroundService
{
    private readonly IBot bot = new Bot(
        new DiscordSocketClient(
            DiscordSocketConfigFactory.GetConfig()));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // Make discord bot available to IDiscordSocketClientAccessor
            ((DiscordBotAccessor)discordSocketClientAccessor)
                .SetDiscordBot(this.bot);
            this.bot.Socket.Log += this.LogAsync;
            this.bot.Socket.UserVoiceStateUpdated += this.UserVoiceStateUpdatedAsync;

            await this.bot.Socket.LoginAsync(
                TokenType.Bot,
                discordBotOptions.Value.Token);

            await this.bot.Socket.StartAsync();

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            logger.LogInformation(
                "DiscordBot is aware of the following guilds:\n{Guilds}",
                string.Join('\n', this.bot.Socket.Guilds.Select(g => $"\t{g.Name}:{g.Id}")));
            
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (TaskCanceledException)
        {
            await this.bot.Socket.DisposeAsync();
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

    private async Task UserVoiceStateUpdatedAsync(SocketUser user, SocketVoiceState before, SocketVoiceState after)
    {
        // Check if the user is the bot itself
        if (user.Id != this.bot.Socket.CurrentUser.Id)
        {
            return;
        }

        // If the bot joined a new voice channel
        if (after.VoiceChannel != null && after.VoiceChannel != before.VoiceChannel)
        {
            logger.LogInformation("Bot joined voice channel: {Channel}", after.VoiceChannel.Name);
            this.bot.ConnectedVoiceChannels[after.VoiceChannel.Guild.Id] = new VoiceChannel
            {
                SocketVoiceChannel = after.VoiceChannel,
            };
        }
        else if (before.VoiceChannel != null && after.VoiceChannel == null)
        {
            // If the bot left a voice channel
            logger.LogInformation("Bot left voice channel: {Channel}", before.VoiceChannel.Name);
            this.bot.ConnectedVoiceChannels.Remove(before.VoiceChannel.Guild.Id);
        }

        var channels = this.bot.ConnectedVoiceChannels.Values
            .Select(vc => DiscordGuildMapper.Map(vc.SocketVoiceChannel))
            .ToList();

        await hubContext.Clients.All.ConnectedVoiceChannels(channels);
    }
}