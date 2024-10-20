using Discord;
using Discord.WebSocket;
using Domain.Abstraction;
using Domain.Dto;
using Domain.Dto.Discord;
using Interface.Accessor;
using Interface.Service;
using Microsoft.Extensions.Logging;

namespace Implementation.Service;

public class DiscordBotService(
    ILogger<DiscordBotService> logger,
    IUserContextAccessor userContext,
    IDiscordSocketClientAccessor clientAccessor) : IDiscordBotService
{
    public Result<ServiceResponse<List<DiscordGuildDto>>> GetActiveGuilds()
    {
        try
        {
            var guildDtos = new List<DiscordGuildDto>();
            var activeGuilds = clientAccessor.Client.Guilds.ToList();
            foreach (var guild in activeGuilds)
            {
                var channels = guild.TextChannels
                    .Where(tc => tc.GetChannelType() == ChannelType.Text)
                    .OrderBy(tc => $"{tc.Category} {tc.Name}")
                    .Select(Map)
                    .ToList();

                var guildDto = new DiscordGuildDto(
                    GuildId: guild.Id.ToString(),
                    GuildCreatorId: guild.OwnerId.ToString(),
                    GuildName: guild.Name,
                    GuildIconUrl: guild.IconUrl,
                    Members: guild.Users.OrderBy(u => u.Username).Select(Map).ToList(),
                    TextChannels: channels);
                
                guildDtos.Add(guildDto);
            }

            return new ServiceResponse<List<DiscordGuildDto>>(guildDtos);
        }
        catch (System.Exception e)
        {
            return new ResultError(
                ErrorType.Exception,
                "Failed to get active guilds",
                e);
        }
    }

    public async Task<Result<ServiceResponse>> SendMessage(SendDiscordMessageDto sendDiscordMessage)
    {
        var userContextResult = userContext.GetUserContext();
        if (userContextResult.IsError)
        {
            return userContextResult.Error!;
        }

        try
        {
            var user = userContextResult.Unwrap().User;
            var guild = clientAccessor.Client.GetGuild(ulong.Parse(sendDiscordMessage.GuildId));
            if (guild is null)
            {
                return new ServiceResponse("Guild not found");
            }

            var channel = await clientAccessor.Client
                .GetChannelAsync(ulong.Parse(sendDiscordMessage.TextChannelId));
            if (channel is null)
            {
                return new ServiceResponse("Channel not found");
            }

            var mentionName = user.AuthenticationMethod == "discord"
                ? $"<@{user.AuthenticationId}>"
                : user.Username;

            var text = $"{mentionName}\n{sendDiscordMessage.Message}";
            var msg = await (channel as SocketTextChannel)!.SendMessageAsync(text);

            logger.LogInformation(
                "{User} sent discord-message \"{Message}\" in '{Guild}' -> '{Channel}'",
                user.Username,
                msg.Content,
                guild.Name,
                channel.Name);

            return new ServiceResponse();
        }
        catch (System.Exception e)
        {
            return new ResultError(
                ErrorType.Exception,
                "Failed to send message",
                e);
        }
    }

    private static DiscordTextChannelDto Map(SocketTextChannel socketTextChannel)
    {
        return new DiscordTextChannelDto(
            TextChannelId: socketTextChannel.Id.ToString(),
            ChannelName: socketTextChannel.Name,
            Category: string.IsNullOrWhiteSpace(socketTextChannel.Category.Name) ? null : socketTextChannel.Category.Name,
            Members: socketTextChannel.Users.OrderBy(u => u.Username).Select(Map).ToList());
    }

    private static DiscordUserDto Map(SocketGuildUser guildUser)
    {
        return new DiscordUserDto(
            UserId: guildUser.Id.ToString(),
            Username: guildUser.Username,
            UserAvatarUrl: guildUser.GetAvatarUrl());
    }
}