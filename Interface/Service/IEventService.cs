using Domain.Abstraction;
using Domain.Dto;
using Domain.Dto.Event;

namespace Interface.Service;

public interface IEventService
{
    Task<Result<ServiceResponse<List<EventDto>>>> GetEvents();

    Task<Result<ServiceResponse<EventDto>>> AddEvent(CreateEventDto createEvent);

    Task<Result<ServiceResponse>> RemoveEvent(long eventId);

    Task<Result<ServiceResponse<EventDto>>> EditEvent(EditEventDto editEvent);
}