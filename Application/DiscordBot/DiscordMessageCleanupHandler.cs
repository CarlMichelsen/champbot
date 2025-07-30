using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Presentation.DiscordBot;

namespace Application.DiscordBot;

public class DiscordMessageCleanupHandler(
    ILogger<DiscordMessageCleanupHandler> logger,
    DatabaseContext databaseContext) : IDiscordMessageCleanupHandler
{
    public async Task HandleCleanup()
    {
        logger.LogInformation("Running cleanup handler");
        var cutoffTime = DateTime.UtcNow.AddDays(-30);
        var messagesToBeDeleted = await databaseContext
            .Message
            .Where(m => m.SentAt < cutoffTime)
            .ToListAsync();

        foreach (var message in messagesToBeDeleted)
        {
            logger.LogInformation("Message {messageId} cleaned up", message.Id.Value);
            databaseContext.Message.Remove(message);
        }

        await databaseContext.SaveChangesAsync();
    }
}