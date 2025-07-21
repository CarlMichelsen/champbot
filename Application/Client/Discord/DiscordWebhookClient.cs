using System.Net.Http.Json;
using Application.Configuration.Options;
using Microsoft.Extensions.Options;
using Presentation.Client.Discord;

namespace Application.Client.Discord;

public class DiscordWebhookClient(
    HttpClient httpClient,
    IOptions<ChampBotOptions> options) : IDiscordWebhookClient
{
    public async Task SendMessageAsync(
        WebhookMessage message,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            options.Value.Discord.WebhookUrl,
            message,
            cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}