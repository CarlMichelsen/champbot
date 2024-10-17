using Database.Entity;
using Domain.Abstraction;

namespace Interface.Repository;

public interface IEventRepository
{
    Task<Result<EventEntity>> AddEvent(
        long userId,
        string eventName,
        DateTime eventTimeUtc);

    Task<Result<EventEntity>> EditEvent(
        long userId,
        long eventId,
        DateTime? eventTimeUtc,
        string? eventName);

    Task<Result<List<EventEntity>>> GetEvents(long userId);

    Task<Result> RemoveEvent(long userId, long eventId);
}