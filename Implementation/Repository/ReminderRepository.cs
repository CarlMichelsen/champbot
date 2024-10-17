using Database;
using Database.Entity;
using Domain.Abstraction;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Repository;

public class ReminderRepository(
    DatabaseContext databaseContext) : IReminderRepository
{
    public async Task<Result<ReminderEntity>> AddReminder(
        long userId,
        long eventId,
        string? reminderNote,
        TimeSpan timeBeforeEvent)
    {
        try
        {
            var parentEvent = await databaseContext.Event
                .Include(e => e.Reminders)
                .FirstOrDefaultAsync(e => e.UserId == userId && e.Id == eventId);
            
            if (parentEvent is null)
            {
                return new ResultError(
                    ErrorType.NotFound,
                    "Did not find event");
            }

            var newReminder = new ReminderEntity
            {
                ReminderNote = reminderNote,
                EventId = eventId,
                Event = parentEvent,
                TimeBeforeEvent = timeBeforeEvent,
                Reminded = false,
                CreatedUtc = DateTime.UtcNow,
            };

            parentEvent.Reminders.Add(newReminder);
            await databaseContext.SaveChangesAsync();

            return newReminder;
        }
        catch (System.Exception e)
        {
            return new ResultError(
                ErrorType.Exception,
                "Error when updating database",
                e);
        }
    }

    public async Task<Result<ReminderEntity>> EditReminder(
        long userId,
        long eventId,
        long reminderId,
        string? reminderNote,
        TimeSpan? timeBeforeEvent)
    {
        try
        {
            var parentEvent = await databaseContext.Event
                .Include(e => e.Reminders)
                .FirstOrDefaultAsync(e => e.UserId == userId && e.Id == eventId);
            
            if (parentEvent is null)
            {
                return new ResultError(
                    ErrorType.NotFound,
                    "Did not find event");
            }

            var exsistingReminder = parentEvent.Reminders.FirstOrDefault(r => r.Id == reminderId);
            if (exsistingReminder is null)
            {
                return new ResultError(
                    ErrorType.NotFound,
                    "Did not find reminder");
            }

            if (reminderNote is not null)
            {
                exsistingReminder.ReminderNote = reminderNote.Length == 0
                    ? null
                    : reminderNote;
            }

            if (timeBeforeEvent is not null)
            {
                exsistingReminder.TimeBeforeEvent = (TimeSpan)timeBeforeEvent;
            }

            await databaseContext.SaveChangesAsync();
            return exsistingReminder;
        }
        catch (System.Exception e)
        {
            return new ResultError(
                ErrorType.Exception,
                "Error when updating database",
                e);
        }
    }

    public async Task<Result> RemoveReminder(
        long userId,
        long eventId,
        long reminderId)
    {
        try
        {
            var parentEvent = await databaseContext.Event
                .Include(e => e.Reminders)
                .FirstOrDefaultAsync(e => e.UserId == userId && e.Id == eventId);
            
            if (parentEvent is null)
            {
                return new ResultError(
                    ErrorType.NotFound,
                    "Did not find event");
            }

            var exsistingReminder = parentEvent.Reminders.FirstOrDefault(r => r.Id == reminderId);
            if (exsistingReminder is null)
            {
                return new ResultError(
                    ErrorType.NotFound,
                    "Did not find reminder");
            }

            parentEvent.Reminders.Remove(exsistingReminder);
            await databaseContext.SaveChangesAsync();

            return new Result();
        }
        catch (System.Exception e)
        {
            return new ResultError(
                ErrorType.Exception,
                "Error when deleting from database",
                e);
        }
    }
}