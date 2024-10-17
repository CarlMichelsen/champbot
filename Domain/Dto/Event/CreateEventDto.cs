namespace Domain.Dto.Event;

public record CreateEventDto(
    string EventName,
    string? ReminderNote,
    DateTime EventTimeUtc);