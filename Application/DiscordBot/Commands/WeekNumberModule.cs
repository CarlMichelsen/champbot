using System.Globalization;
using Discord.Interactions;

namespace Application.DiscordBot.Commands;

public class WeekNumberModule(
    TimeProvider timeProvider) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("week", "Current week number")]
    public async Task GetWeekNumber()
    {
        var weekNumber = ISOWeek.GetWeekOfYear(timeProvider.GetUtcNow().DateTime);
        var response = $"Current week: {weekNumber}";
        await this.RespondAsync(response, ephemeral: true);
    }
}