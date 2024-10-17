namespace Domain.Dto.Event;

public record EventDto(
    long Id,
    string EventName,
    DateTime EventTimeUtc,
    List<ReminderDto> Reminders,
    DateTime CreatedUtc);