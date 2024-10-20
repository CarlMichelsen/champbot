using Domain.Dto.Discord;
using Microsoft.AspNetCore.Http;

namespace Interface.Handler;

public interface IDiscordBotHandler
{
    IResult GetActiveGuilds();

    Task<IResult> SendMessage(SendDiscordMessageDto sendDiscordMessage);
}