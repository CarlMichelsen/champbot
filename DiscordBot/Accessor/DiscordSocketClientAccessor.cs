using Discord.WebSocket;
using Interface.Accessor;

namespace DiscordBot.Accessor;

public class DiscordSocketClientAccessor : IDiscordSocketClientAccessor
{
    private DiscordSocketClient? client;

    public DiscordSocketClient Client => this.client
        ?? throw new NullReferenceException(
            "Attempted to access DiscordSocketClient when none had been set");

    internal void SetDiscordSocketClient(DiscordSocketClient newClient)
    {
        this.client ??= newClient;
    }
}