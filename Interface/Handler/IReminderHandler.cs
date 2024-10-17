using Domain.Dto.Event;
using Microsoft.AspNetCore.Http;

namespace Interface.Handler;

public interface IReminderHandler
{
    Task<IResult> AddReminder(CreateReminderDto createReminder);

    Task<IResult> RemoveReminder(long eventId, long reminderId);

    Task<IResult> EditReminder(EditReminderDto editReminder);
}