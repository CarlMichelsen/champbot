using Database;
using Database.Entity;
using Database.Entity.Id;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Presentation.DiscordBot;

namespace Application.DiscordBot;

public class SocketMessageHandler(
    ILogger<SocketMessageHandler> logger,
    DatabaseContext databaseContext) : ISocketMessageHandler
{
    public async Task HandleSocketMessage(
        SocketMessage socketMessage,
        CancellationToken cancellationToken = default)
    {
        var sender = socketMessage.Author;
        var message = await socketMessage.Channel.GetMessageAsync(socketMessage.Id);

        SocketGuild? guild = null;
        if (message.Channel is SocketGuildChannel guildChannel)
        {
            guild = guildChannel.Guild;
        }

        var messageEntity = new MessageEntity
        {
            Id = new MessageEntityId(Guid.CreateVersion7()),
            MessageDiscordId = message.Id,
            SenderDiscordId = sender.Id,
            GuildDiscordId = guild?.Id,
            Source = MessageSourceToString(message.Source),
            Content = message.Content,
            SentAt = message.CreatedAt.DateTime.ToUniversalTime(),
        };
                
        logger.LogInformation(
            "[{GuildName}]\n{Username} sent message -> '{Content}'",
            guild?.Name ?? "None",
            sender.Username,
            message.Content);
                
        await databaseContext.Message.AddAsync(messageEntity, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
    }
    
    private static string MessageSourceToString(MessageSource source)
        => source switch
        {
            MessageSource.System => "SYSTEM",
            MessageSource.User => "USER",
            MessageSource.Bot => "BOT",
            MessageSource.Webhook => "WEBHOOK",
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
        };
}