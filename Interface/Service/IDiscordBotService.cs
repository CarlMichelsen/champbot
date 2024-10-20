using Domain.Abstraction;
using Domain.Dto;
using Domain.Dto.Discord;

namespace Interface.Service;

public interface IDiscordBotService
{
    Result<ServiceResponse<List<DiscordGuildDto>>> GetActiveGuilds();

    Task<Result<ServiceResponse>> SendMessage(SendDiscordMessageDto sendDiscordMessage);
}