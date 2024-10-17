using Domain.Dto.Event;
using Interface.Handler;
using Interface.Service;
using Microsoft.AspNetCore.Http;

namespace Implementation.Handler;

public class ReminderHandler(
    IReminderService reminderService) : IReminderHandler
{
    public async Task<IResult> AddReminder(CreateReminderDto createReminder)
    {
        var res = await reminderService.AddReminder(createReminder);
        return Results.Ok(res);
    }

    public async Task<IResult> EditReminder(EditReminderDto editReminder)
    {
        var res = await reminderService.EditReminder(editReminder);
        return Results.Ok(res);
    }

    public async Task<IResult> RemoveReminder(long eventId, long reminderId)
    {
        var res = await reminderService.RemoveReminder(eventId, reminderId);
        return Results.Ok(res);
    }
}