using System.Threading.Channels;
using Discord.WebSocket;
using Presentation.DiscordBot;

namespace Api.HostedServices;

public class MessageProcessor(
    ILogger<MessageProcessor> logger,
    Channel<SocketMessage> channel,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<ISocketMessageHandler>();
                var socketMessage = await channel.Reader.ReadAsync(stoppingToken);
                
                // Actually handle socket-message
                await handler.HandleSocketMessage(socketMessage, stoppingToken);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, e.Message);
            }
        }
    }
}