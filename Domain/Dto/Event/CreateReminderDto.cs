namespace Domain.Dto.Event;

public record CreateReminderDto(
    long EventId,
    string? ReminderNote,
    long MinutesBeforeEvent);