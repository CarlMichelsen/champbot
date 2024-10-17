namespace Domain.Dto.Event;

public record ReminderDto(
    long Id,
    long EventId,
    string? ReminderNote,
    TimeSpan TimeBeforeEvent,
    bool Reminded,
    DateTime CreatedUtc);