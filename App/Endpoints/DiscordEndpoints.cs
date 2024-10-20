using Domain.Dto.Discord;
using Interface.Handler;
using Microsoft.AspNetCore.Mvc;

namespace App.Endpoints;

public static class DiscordEndpoints
{
    public static void RegisterDiscordEndpoints(
        this IEndpointRouteBuilder apiGroup)
    {
        var discordgroup = apiGroup
            .MapGroup("discord")
            .WithTags("Discord");
        
        discordgroup.MapGet(
            string.Empty,
            ([FromServices] IDiscordBotHandler handler) =>
                handler.GetActiveGuilds());
        
        discordgroup.MapPost(
            string.Empty,
            async ([FromServices] IDiscordBotHandler handler, [FromBody] SendDiscordMessageDto sendDiscordMessage) =>
                await handler.SendMessage(sendDiscordMessage));
    }
}