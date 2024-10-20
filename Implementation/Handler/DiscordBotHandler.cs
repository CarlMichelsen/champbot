using Domain.Dto.Discord;
using Interface.Handler;
using Interface.Service;
using Microsoft.AspNetCore.Http;

namespace Implementation.Handler;

public class DiscordBotHandler(
    IResultErrorLogService errorLog,
    IDiscordBotService discordBotService) : IDiscordBotHandler
{
    public IResult GetActiveGuilds()
    {
        var res = discordBotService.GetActiveGuilds();
        if (res.IsError)
        {
            errorLog.Log(res);
            return Results.StatusCode(500);
        }

        return Results.Ok(res.Unwrap());
    }

    public async Task<IResult> SendMessage(
        SendDiscordMessageDto sendDiscordMessage)
    {
        var res = await discordBotService.SendMessage(sendDiscordMessage);
        if (res.IsError)
        {
            errorLog.Log(res);
            return Results.StatusCode(500);
        }

        return Results.Ok(res.Unwrap());
    }
}