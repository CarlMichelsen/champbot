using Database.Entity;
using Domain.Abstraction;

namespace Interface.Repository;

public interface IReminderRepository
{
    Task<Result<ReminderEntity>> AddReminder(
        long userId,
        long eventId,
        string? reminderNote,
        TimeSpan timeBeforeEvent);

    Task<Result<ReminderEntity>> EditReminder(
        long userId,
        long eventId,
        long reminderId,
        string? reminderNote,
        TimeSpan? timeBeforeEvent);

    Task<Result> RemoveReminder(
        long userId,
        long eventId,
        long reminderId);
}