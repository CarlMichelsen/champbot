namespace Domain.Dto.Event;

public record EventDto(
    long Id,
    long CreatorId,
    string EventName,
    DateTime EventTimeUtc,
    List<ReminderDto> Reminders,
    DateTime CreatedUtc);