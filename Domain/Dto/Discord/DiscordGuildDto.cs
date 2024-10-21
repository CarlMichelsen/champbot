namespace Domain.Dto.Discord;

public record DiscordGuildDto(
    string GuildId,
    string GuildCreatorId,
    string GuildName,
    string? GuildIconUrl,
    List<DiscordUserDto> Members,
    List<DiscordChannelDto> VoiceChannels,
    List<DiscordChannelDto> TextChannels);