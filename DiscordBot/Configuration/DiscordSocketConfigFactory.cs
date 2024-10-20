using Discord;
using Discord.WebSocket;

namespace DiscordBot.Configuration;

public static class DiscordSocketConfigFactory
{
    public static DiscordSocketConfig GetConfig() => new()
    {
        GatewayIntents = GatewayIntents.All,
    };
}