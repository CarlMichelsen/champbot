namespace Domain.Dto.Event;

public record ReminderDto(
    long Id,
    long EventId,
    string? ReminderNote,
    long MinutesBeforeEvent,
    bool Reminded,
    DateTime CreatedUtc);