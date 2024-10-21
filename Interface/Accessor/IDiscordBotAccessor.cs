using Interface.Discord;

namespace Interface.Accessor;

public interface IDiscordBotAccessor
{
    IBot Bot { get; }
}