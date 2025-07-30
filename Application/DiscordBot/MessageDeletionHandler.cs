using System.Text.Json;
using Database;
using Database.Entity;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Presentation.DiscordBot;

namespace Application.DiscordBot;

public class MessageDeletionHandler(
    ILogger<MessageDeletionHandler> logger,
    TimeProvider timeProvider,
    DiscordSocketClient discordSocketClient,
    DatabaseContext databaseContext) : IMessageDeletionHandler
{
    public async Task HandleMessageDeletion(Cacheable<IMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel)
    {
        if (channel.Value is not SocketGuildChannel guildChannel)
        {
            logger.LogWarning(
                "Message <{Id}> was not in a {ChannelTypeName} when deleted - will not handle this deletion event",
                message.Id,
                nameof(SocketGuildChannel));
            return;
        }
        
        var dbMessage = await this.GetMessageEntityByMessageDiscordId(message.Id);
        if (dbMessage is null)
        {
            logger.LogInformation(
                "Message <{Id}> was untracked when deleted - will not handle this deletion event",
                message.Id);
            return;
        }
        
        var auditLogs = await guildChannel.Guild
            .GetAuditLogsAsync(limit: 1, actionType: ActionType.MessageDeleted)
            .FlattenAsync();
    
        var deletion = auditLogs.FirstOrDefault();
        if (deletion is null)
        {
            // Deletion was done by the sender - ignore this case
            return;
        }

        if (deletion.CreatedAt - timeProvider.GetUtcNow() > TimeSpan.FromMinutes(1))
        {
            // Auditlog may not be from this deletion event
            return;
        }
        
        // Record the message as deleted
        dbMessage.DeletedAt = timeProvider.GetUtcNow().DateTime.ToUniversalTime();
        await databaseContext.SaveChangesAsync();

        if (deletion.User.IsBot)
        {
            // Ignore deleted bot messages
            return;
        }
        
        if (deletion.User.Id == dbMessage.SenderDiscordId)
        {
            // Ignore admins deleting their own messages (I'm not sure if this would ever count as an audit-logged deletion)
            return;
        }

        if (guildChannel is SocketTextChannel textChannel)
        {
            var deletingUserId = deletion.User.Id;
            var timeString = dbMessage.SentAt.ToLocalTime().Humanize(utcDate: true); // There is something strange going on with utc time here
            await textChannel.SendMessageAsync($"<@{deletingUserId}> just used admin privileges to delete a message originally sent in the <#{textChannel.Id}> channel by <@{dbMessage.SenderDiscordId}> {timeString}.");
        }
    }

    private Task<MessageEntity?> GetMessageEntityByMessageDiscordId(ulong messageId)
    {
        return databaseContext.Message.FirstOrDefaultAsync(m => m.MessageDiscordId == messageId);
    }
}