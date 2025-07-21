using Presentation.Configuration.Options;

namespace Application.Configuration.Options;

public class ApplicationOptions : IConfigurationOptions
{
    public static string SectionName { get; } = "Application";
    
    public required DiscordWebhookOptions DiscordWebhookWebhook { get; init; }
}