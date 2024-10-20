namespace Domain.Dto.Discord;

public record DiscordUserDto(
    string UserId,
    string Username,
    string? UserAvatarUrl);