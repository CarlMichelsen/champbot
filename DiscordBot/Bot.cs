using Discord.WebSocket;
using Domain.Discord;
using Interface.Discord;

namespace DiscordBot;

public class Bot(DiscordSocketClient socket) : IBot
{
    public DiscordSocketClient Socket => socket;

    public Dictionary<ulong, VoiceChannel> ConnectedVoiceChannels { get; init; } = [];
}