using Domain.User;
using Implementation.Util;
using Interface.Accessor;
using Interface.Hubs;
using Interface.Service;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Hubs;

[Microsoft.AspNetCore.Authorization.Authorize]
public class VoiceHub(
    ILogger<VoiceHub> logger,
    IDiscordBotAccessor botAccessor,
    IUserContextAccessor userContextAccessor,
    IDiscordBotService discordBotService) : Hub<IVoiceClientMethods>
{
    private UserContext UserContext => (UserContext)this.Context.Items["userContext"]!;

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        var userContextResult = userContextAccessor.GetUserContext(this.Context.GetHttpContext()!);
        if (userContextResult.IsError)
        {
            this.Context.Abort();
        }

        this.Context.Items["userContext"] = userContextResult.Unwrap();
        logger.LogInformation("{Username} Connected", this.UserContext.User.Username);
    }

    public async Task GetGuildData()
    {
        var guildsResult = discordBotService.GetActiveGuilds();
        if (guildsResult.IsError)
        {
            return;
        }

        if (!guildsResult.Unwrap().Ok)
        {
            return;
        }

        await this.Clients.Caller.Guilds(guildsResult.Unwrap().Value!);
        
        var channels = botAccessor.Bot.ConnectedVoiceChannels.Values
            .Select(vc => DiscordGuildMapper.Map(vc.SocketVoiceChannel))
            .ToList();
        if (channels.Count > 0)
        {
            await this.Clients.All.ConnectedVoiceChannels(channels);
        }
    }

    public async Task ConnectToVoice(string guildIdString, string channelIdString)
    {
        try
        {
            var guildId = ulong.Parse(guildIdString);
            var channelId = ulong.Parse(channelIdString);
            var socket = botAccessor.Bot.Socket;

            var guild = socket.GetGuild(guildId);
            if (guild == null)
            {
                return;
            }

            var channel = guild.GetVoiceChannel(channelId);
            if (channel == null)
            {
                return;
            }

            var audioClient = await channel.ConnectAsync();
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            if (botAccessor.Bot.ConnectedVoiceChannels.TryGetValue(guild.Id, out var vc))
            {
                vc.AudioClient = audioClient;
            }
        }
        catch (System.Exception e)
        {
            logger.LogCritical(e, "ConnectToVoice Hub method threw an exception");
        }
    }

    public async Task LeaveGuildVoice(string guildIdString)
    {
        try
        {
            var guildId = ulong.Parse(guildIdString);
            await this.LeaveAllVoiceChannelsInGuild(guildId);
        }
        catch (System.Exception e)
        {
            logger.LogCritical(e, "LeaveGuildVoice Hub method threw an exception");
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        logger.LogInformation("{Username} Disconnected", this.UserContext.User.Username);
    }

    private async Task LeaveAllVoiceChannelsInGuild(ulong guildId)
    {
        var socket = botAccessor.Bot.Socket;
        var guild = socket.GetGuild(guildId);
        if (guild == null)
        {
            return;
        }

        var voiceState = guild.CurrentUser.VoiceState;
        if (voiceState is not null)
        {
            await voiceState.Value.VoiceChannel.DisconnectAsync();
        }
    }
}