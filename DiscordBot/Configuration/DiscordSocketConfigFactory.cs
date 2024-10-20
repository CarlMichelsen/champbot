using Discord.WebSocket;

namespace DiscordBot.Configuration;

public static class DiscordSocketConfigFactory
{
    public static DiscordSocketConfig GetConfig() => new()
    {
        GatewayIntents = Discord.GatewayIntents.All,
    };
}