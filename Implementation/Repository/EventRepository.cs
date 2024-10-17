using Database;
using Database.Entity;
using Domain.Abstraction;
using Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Repository;

public class EventRepository(
    DatabaseContext databaseContext) : IEventRepository
{
    public async Task<Result<EventEntity>> AddEvent(
        long userId,
        string eventName,
        DateTime eventTimeUtc)
    {
        try
        {
            var newEvent = new EventEntity
            {
                EventName = eventName,
                EventTimeUtc = eventTimeUtc,
                Reminders = [],
                CreatedUtc = DateTime.UtcNow,
                UserId = userId,
            };

            await databaseContext.Event.AddAsync(newEvent);
            await databaseContext.SaveChangesAsync();
            return newEvent;
        }
        catch (System.Exception e)
        {
            return new ResultError(
                ErrorType.Exception,
                "Error when updating database",
                e);
        }
    }

    public async Task<Result<EventEntity>> EditEvent(
        long userId,
        long eventId,
        DateTime? eventTimeUtc,
        string? eventName)
    {
        try
        {
            var eventToEdit = await databaseContext.Event
                .FirstOrDefaultAsync(e => e.UserId == userId && e.Id == eventId);

            if (eventToEdit is null)
            {
                return new ResultError(
                    ErrorType.NotFound,
                    "Did not find event");
            }
            
            if (eventTimeUtc is not null)
            {
                eventToEdit.EventTimeUtc = (DateTime)eventTimeUtc;
            }

            if (eventName is not null)
            {
                eventToEdit.EventName = eventName;
            }

            await databaseContext.SaveChangesAsync();
            return eventToEdit;
        }
        catch (System.Exception e)
        {
            return new ResultError(
                ErrorType.Exception,
                "Error when updating database",
                e);
        }
    }

    public async Task<Result<List<EventEntity>>> GetEvents(long userId)
    {
        try
        {
            var events = await databaseContext.Event
                .Where(e => e.UserId == userId)
                .Include(e => e.Reminders)
                .ToListAsync();

            if (events is null)
            {
                return new ResultError(
                    ErrorType.NotFound,
                    "Did not find events");
            }
            
            return events;
        }
        catch (System.Exception e)
        {
            return new ResultError(
                ErrorType.Exception,
                "Error when reading database",
                e);
        }
    }

    public async Task<Result> RemoveEvent(long userId, long eventId)
    {
        try
        {
            var deleteCount = await databaseContext.Event
                .Where(e => e.UserId == userId && e.Id == eventId)
                .ExecuteDeleteAsync();
            
            if (deleteCount != 1)
            {
                return new ResultError(
                    ErrorType.NotFound,
                    "Did not find event to delete");
            }

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