namespace Domain.Dto.Event;

public record CreateEventDto(
    string EventName,
    DateTime EventTimeUtc);