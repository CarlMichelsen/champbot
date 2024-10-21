namespace Domain.Dto.Discord;

public record DiscordChannelDto(
    string GuildId,
    string ChannelId,
    string ChannelName,
    string? Category,
    List<DiscordUserDto> Members);