using Discord.WebSocket;
using Domain.Discord;

namespace Interface.Discord;

public interface IBot
{
    DiscordSocketClient Socket { get; }

    public Dictionary<ulong, VoiceChannel> ConnectedVoiceChannels { get; }
}