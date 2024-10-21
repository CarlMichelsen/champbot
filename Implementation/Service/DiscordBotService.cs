using Discord;
using Discord.WebSocket;
using Domain.Abstraction;
using Domain.Dto;
using Domain.Dto.Discord;
using Implementation.Util;
using Interface.Accessor;
using Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Implementation.Service;

public class DiscordBotService(
    ILogger<DiscordBotService> logger,
    IUserContextAccessor userContext,
    IDiscordBotAccessor botAccessor) : IDiscordBotService
{
    public Result<ServiceResponse<List<DiscordGuildDto>>> GetActiveGuilds(
        HttpContext? httpContext = default)
    {
        try
        {
            var guildDtos = new List<DiscordGuildDto>();
            var activeGuilds = botAccessor.Bot.Socket.Guilds.ToList();
            foreach (var guild in activeGuilds)
            {
                var voiceChannels = guild.VoiceChannels
                    .Where(tc => tc.GetChannelType() == ChannelType.Voice)
                    .OrderBy(tc => $"{tc.Category} {tc.Name}")
                    .Select(DiscordGuildMapper.Map)
                    .ToList();

                var textChannels = guild.TextChannels
                    .Where(tc => tc.GetChannelType() == ChannelType.Text)
                    .OrderBy(tc => $"{tc.Category} {tc.Name}")
                    .Select(DiscordGuildMapper.Map)
                    .ToList();

                var guildDto = new DiscordGuildDto(
                    GuildId: guild.Id.ToString(),
                    GuildCreatorId: guild.OwnerId.ToString(),
                    GuildName: guild.Name,
                    GuildIconUrl: guild.IconUrl,
                    Members: guild.Users.OrderBy(u => u.Username).Select(DiscordGuildMapper.Map).ToList(),
                    VoiceChannels: voiceChannels,
                    TextChannels: textChannels);
                
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

    public async Task<Result<ServiceResponse>> SendMessage(
        SendDiscordMessageDto sendDiscordMessage,
        HttpContext? httpContext = default)
    {
        var userContextResult = userContext.GetUserContext();
        if (userContextResult.IsError)
        {
            return userContextResult.Error!;
        }

        try
        {
            var user = userContextResult.Unwrap().User;
            var guild = botAccessor.Bot.Socket.Guilds
                .FirstOrDefault(g => g.Id == ulong.Parse(sendDiscordMessage.GuildId));
            if (guild is null)
            {
                return new ServiceResponse("Guild not found");
            }

            var channel = await botAccessor.Bot.Socket
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
}