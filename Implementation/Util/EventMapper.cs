using Database.Entity;
using Domain.Abstraction;
using Domain.Dto.Event;

namespace Implementation.Util;

public static class EventMapper
{
    public static Result<EventDto> MapEvent(EventEntity eventEntity)
    {
        try
        {
            var reminderResults = eventEntity.Reminders
                .Select(MapReminder)
                .ToList();
            
            var mapError = reminderResults.Exists(r => r.IsError);
            if (mapError)
            {
                return new ResultError(
                    ErrorType.Exception,
                    "map error");
            }

            return new EventDto(
                Id: eventEntity.Id,
                EventName: eventEntity.EventName,
                EventTimeUtc: eventEntity.EventTimeUtc,
                Reminders: reminderResults.Select(r => r.Unwrap()).ToList(),
                CreatedUtc: eventEntity.CreatedUtc);
        }
        catch (System.Exception e)
        {
            return new ResultError(
                ErrorType.Exception,
                "map error",
                e);
        }
    }

    public static Result<ReminderDto> MapReminder(ReminderEntity reminderEntity)
    {
        try
        {
            return new ReminderDto(
                Id: reminderEntity.Id,
                EventId: reminderEntity.EventId,
                ReminderNote: reminderEntity.ReminderNote,
                TimeBeforeEvent: reminderEntity.TimeBeforeEvent,
                Reminded: reminderEntity.Reminded,
                CreatedUtc: reminderEntity.CreatedUtc);
        }
        catch (System.Exception e)
        {
            return new ResultError(
                ErrorType.Exception,
                "map error",
                e);
        }
    }
}