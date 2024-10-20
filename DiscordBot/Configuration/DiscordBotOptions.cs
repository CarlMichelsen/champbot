namespace DiscordBot.Configuration;

public class DiscordBotOptions
{
    public const string SectionName = "DiscordBot";

    public required string Token { get; init; }
}