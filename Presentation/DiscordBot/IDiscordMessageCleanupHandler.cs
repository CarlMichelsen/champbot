namespace Presentation.DiscordBot;

public interface IDiscordMessageCleanupHandler
{
    Task HandleCleanup();
}