using Presentation.Configuration.Options;

namespace Application.Configuration.Options;

public class DiscordBotOptions : IConfigurationOptions
{
    public static string SectionName { get; } = "DiscordBot";
    
    public required string Token { get; init; }
    
    public ulong? DebugGuild { get; init; }
}