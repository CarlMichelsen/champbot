namespace Domain.Dto.Discord;

public record DiscordTextChannelDto(
    string TextChannelId,
    string ChannelName,
    string? Category,
    List<DiscordUserDto> Members);