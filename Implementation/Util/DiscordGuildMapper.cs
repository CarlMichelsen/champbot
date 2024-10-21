using Discord.WebSocket;
using Domain.Dto.Discord;

namespace Implementation.Util;

public static class DiscordGuildMapper
{
    public static DiscordChannelDto Map(SocketTextChannel socketTextChannel)
    {
        return new DiscordChannelDto(
            GuildId: socketTextChannel.Guild.Id.ToString(),
            ChannelId: socketTextChannel.Id.ToString(),
            ChannelName: socketTextChannel.Name,
            Category: string.IsNullOrWhiteSpace(socketTextChannel.Category.Name) ? null : socketTextChannel.Category.Name,
            Members: socketTextChannel.Users.OrderBy(u => u.Username).Select(Map).ToList());
    }

    public static DiscordUserDto Map(SocketGuildUser guildUser)
    {
        return new DiscordUserDto(
            UserId: guildUser.Id.ToString(),
            Username: guildUser.Username,
            UserAvatarUrl: guildUser.GetAvatarUrl());
    }
}