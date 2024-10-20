using Discord;
using Discord.WebSocket;

namespace DiscordBot.Configuration;

public static class DiscordSocketConfigFactory
{
    public static DiscordSocketConfig GetConfig() => new()
    {
        GatewayIntents =
            GatewayIntents.AllUnprivileged
            | GatewayIntents.Guilds
            | GatewayIntents.GuildMembers
            | GatewayIntents.GuildMessages
            | GatewayIntents.GuildPresences
            | GatewayIntents.GuildMessageReactions
            | GatewayIntents.MessageContent
            | GatewayIntents.DirectMessages
            | GatewayIntents.DirectMessageReactions,
    };
}