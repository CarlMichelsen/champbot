namespace Domain.Dto.Event;

public record EditEventDto(
    long EventId,
    DateTime? EventTimeUtc,
    string? Name);