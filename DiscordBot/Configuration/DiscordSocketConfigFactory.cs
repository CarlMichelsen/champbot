using Discord;
using Discord.WebSocket;

namespace DiscordBot.Configuration;

public static class DiscordSocketConfigFactory
{
    public static DiscordSocketConfig GetConfig() => new()
    {
        GatewayIntents =
            GatewayIntents.AllUnprivileged
            | GatewayIntents.MessageContent
            | GatewayIntents.DirectMessages
            | GatewayIntents.GuildMembers
            | GatewayIntents.DirectMessages
            | GatewayIntents.DirectMessages
            | GatewayIntents.DirectMessageReactions
            | GatewayIntents.GuildMessages
            | GatewayIntents.GuildMessageReactions,
    };
}