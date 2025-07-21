namespace Application.DiscordBot.Commands;

using System.Globalization;
using Discord.Interactions;

public class WeekNumberModule(
    TimeProvider timeProvider) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("week", "Current week number")]
    public async Task GetWeekNumber()
    {
        var culture = CultureInfo.InvariantCulture;
        var weekNumber = culture.Calendar.GetWeekOfYear(
            timeProvider.GetUtcNow().DateTime,
            culture.DateTimeFormat.CalendarWeekRule,
            culture.DateTimeFormat.FirstDayOfWeek);
        await this.RespondAsync($"Current week: {weekNumber}", ephemeral: true);
    }
}