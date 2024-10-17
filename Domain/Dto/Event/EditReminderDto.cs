namespace Domain.Dto.Event;

public record EditReminderDto(
    long EventId,
    long ReminderId,
    string? ReminderNote,
    TimeSpan? TimeBeforeEvent);