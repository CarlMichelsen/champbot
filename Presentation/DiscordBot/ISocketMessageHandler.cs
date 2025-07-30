using Discord.WebSocket;

namespace Presentation.DiscordBot;

public interface ISocketMessageHandler
{
    Task HandleSocketMessage(
        SocketMessage socketMessage,
        CancellationToken cancellationToken = default);
}