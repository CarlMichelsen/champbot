using System.Reflection;
using Application.DiscordBot;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Api.Extensions;

public static class DiscordBotExtensions
{
    public static WebApplicationBuilder AddDiscordBot(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<DiscordSocketClient>(_ =>
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.Guilds | GatewayIntents.GuildMembers,
                AlwaysDownloadUsers = true,
            };
            
            return new DiscordSocketClient(config);
        });
        
        // Add InteractionService
        builder.Services.AddSingleton<InteractionService>(provider =>
        {
            var client = provider.GetRequiredService<DiscordSocketClient>();
            return new InteractionService(client);
        });
        
        // Register background service
        builder.Services.AddHostedService<DiscordBot>();
        return builder;
    }
}