using Domain.Abstraction;
using Domain.Dto;
using Domain.Dto.Discord;
using Microsoft.AspNetCore.Http;

namespace Interface.Service;

public interface IDiscordBotService
{
    Result<ServiceResponse<List<DiscordGuildDto>>> GetActiveGuilds(HttpContext? httpContext = default);

    Task<Result<ServiceResponse>> SendMessage(SendDiscordMessageDto sendDiscordMessage, HttpContext? httpContext = default);
}