using Domain.Dto;
using Domain.Dto.Event;

namespace Interface.Service;

public interface IEventService
{
    Task<ServiceResponse<List<EventDto>>> GetEvents();

    Task<ServiceResponse<EventDto>> AddEvent(CreateEventDto createEvent);

    Task<ServiceResponse> RemoveEvent(long eventId);

    Task<ServiceResponse<EventDto>> EditEvent(EditEventDto editEvent);
}