using Domain.Dto.Event;
using Microsoft.AspNetCore.Http;

namespace Interface.Handler;

public interface IEventHandler
{
    Task<IResult> GetEvents();

    Task<IResult> AddEvent(CreateEventDto createEvent);

    Task<IResult> RemoveEvent(long eventId);

    Task<IResult> EditEvent(EditEventDto editEvent);
}