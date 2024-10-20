namespace Domain.Dto.Discord;

public record SendDiscordMessageDto(
    string GuildId,
    string TextChannelId,
    string Message);