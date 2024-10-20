using Discord.WebSocket;

namespace Interface.Accessor;

public interface IDiscordSocketClientAccessor
{
    DiscordSocketClient Client { get; }
}