using Domain.Dto.Event;
using Interface.Handler;
using Interface.Service;
using Microsoft.AspNetCore.Http;

namespace Implementation.Handler;

public class EventHandler(
    IEventService eventService) : IEventHandler
{
    public async Task<IResult> AddEvent(CreateEventDto createEvent)
    {
        var res = await eventService.AddEvent(createEvent);
        return Results.Ok(res);
    }

    public async Task<IResult> EditEvent(EditEventDto editEvent)
    {
        var res = await eventService.EditEvent(editEvent);
        return Results.Ok(res);
    }

    public async Task<IResult> GetEvents()
    {
        var res = await eventService.GetEvents();
        return Results.Ok(res);
    }

    public async Task<IResult> RemoveEvent(long eventId)
    {
        var res = await eventService.RemoveEvent(eventId);
        return Results.Ok(res);
    }
}