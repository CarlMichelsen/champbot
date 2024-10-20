using Domain.Dto.Event;
using Interface.Handler;
using Interface.Service;
using Microsoft.AspNetCore.Http;

namespace Implementation.Handler;

public class EventHandler(
    IResultErrorLogService errorLog,
    IEventService eventService) : IEventHandler
{
    public async Task<IResult> AddEvent(CreateEventDto createEvent)
    {
        var res = await eventService.AddEvent(createEvent);
        if (res.IsError)
        {
            errorLog.Log(res);
            return Results.StatusCode(500);
        }

        return Results.Ok(res.Unwrap());
    }

    public async Task<IResult> EditEvent(EditEventDto editEvent)
    {
        var res = await eventService.EditEvent(editEvent);
        if (res.IsError)
        {
            errorLog.Log(res);
            return Results.StatusCode(500);
        }

        return Results.Ok(res.Unwrap());
    }

    public async Task<IResult> GetEvents()
    {
        var res = await eventService.GetEvents();
        if (res.IsError)
        {
            errorLog.Log(res);
            return Results.StatusCode(500);
        }

        return Results.Ok(res.Unwrap());
    }

    public async Task<IResult> RemoveEvent(long eventId)
    {
        var res = await eventService.RemoveEvent(eventId);
        if (res.IsError)
        {
            errorLog.Log(res);
            return Results.StatusCode(500);
        }

        return Results.Ok(res.Unwrap());
    }
}