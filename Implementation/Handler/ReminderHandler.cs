using Domain.Dto.Event;
using Interface.Handler;
using Interface.Service;
using Microsoft.AspNetCore.Http;

namespace Implementation.Handler;

public class ReminderHandler(
    IResultErrorLogService errorLog,
    IReminderService reminderService) : IReminderHandler
{
    public async Task<IResult> AddReminder(CreateReminderDto createReminder)
    {
        var res = await reminderService.AddReminder(createReminder);
        if (res.IsError)
        {
            errorLog.Log(res);
            return Results.StatusCode(500);
        }

        return Results.Ok(res.Unwrap());
    }

    public async Task<IResult> EditReminder(EditReminderDto editReminder)
    {
        var res = await reminderService.EditReminder(editReminder);
        if (res.IsError)
        {
            errorLog.Log(res);
            return Results.StatusCode(500);
        }

        return Results.Ok(res.Unwrap());
    }

    public async Task<IResult> RemoveReminder(long eventId, long reminderId)
    {
        var res = await reminderService.RemoveReminder(eventId, reminderId);
        if (res.IsError)
        {
            errorLog.Log(res);
            return Results.StatusCode(500);
        }

        return Results.Ok(res.Unwrap());
    }
}