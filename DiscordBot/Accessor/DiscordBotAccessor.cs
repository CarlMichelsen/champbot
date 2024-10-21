using Interface.Accessor;
using Interface.Discord;

namespace DiscordBot.Accessor;

public class DiscordBotAccessor : IDiscordBotAccessor
{
    private IBot? bot;

    public IBot Bot => this.bot
        ?? throw new NullReferenceException(
            "Attempted to access DiscordSocketClient when none had been set");

    internal void SetDiscordBot(IBot newBot)
    {
        this.bot ??= newBot;
    }
}