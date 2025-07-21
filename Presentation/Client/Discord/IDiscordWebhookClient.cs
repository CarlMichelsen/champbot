namespace Presentation.Client.Discord;

public interface IDiscordWebhookClient
{
    Task SendMessageAsync(
        WebhookMessage message,
        CancellationToken cancellationToken = default);
}