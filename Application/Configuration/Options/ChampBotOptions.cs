using Presentation.Configuration.Options;

namespace Application.Configuration.Options;

public class ChampBotOptions : IConfigurationOptions
{
    public static string SectionName { get; } = "ChampBot";
    
    public required DiscordOptions Discord { get; init; }
}