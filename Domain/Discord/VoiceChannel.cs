using Discord.Audio;
using Discord.WebSocket;

namespace Domain.Discord;

public class VoiceChannel
{
    public required SocketVoiceChannel SocketVoiceChannel { get; init; }

    public IAudioClient? AudioClient { get; set; }
}