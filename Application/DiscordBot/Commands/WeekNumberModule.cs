namespace Application.DiscordBot.Commands;

using System.Globalization;
using Discord.Interactions;

public class WeekNumberModule(
    TimeProvider timeProvider) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("week", "Current week number")]
    public async Task GetWeekNumber()
    {
        var calendar = CultureInfo.InvariantCulture.Calendar;
        var currentWeek = calendar.GetWeekOfYear(timeProvider.GetUtcNow().DateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        await this.RespondAsync($"Current week: {currentWeek}", ephemeral: true);
    }
}