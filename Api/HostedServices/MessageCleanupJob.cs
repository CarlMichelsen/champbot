using Presentation.DiscordBot;

namespace Api.HostedServices;

public class MessageCleanupJob(
    ILogger<MessageCleanupJob> logger,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IDiscordMessageCleanupHandler>();

                await handler.HandleCleanup();
                
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, e.Message);
                
                // Wait a bit before trying again...
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }
    }
}