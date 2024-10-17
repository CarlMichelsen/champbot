using Domain.Dto.Event;
using Interface.Handler;
using Microsoft.AspNetCore.Mvc;

namespace App.Endpoints;

public static class EventEndpoints
{
    public static void RegisterEventEndpoints(
        this IEndpointRouteBuilder apiGroup)
    {
        var eventgroup = apiGroup
            .MapGroup("event")
            .WithTags("Event");

        eventgroup.MapGet(
            string.Empty,
            async ([FromServices] IEventHandler handler) =>
                await handler.GetEvents());
        
        eventgroup.MapPost(
            string.Empty,
            async ([FromServices] IEventHandler handler, [FromBody] CreateEventDto createEvent) =>
                await handler.AddEvent(createEvent));
        
        eventgroup.MapPut(
            string.Empty,
            async ([FromServices] IEventHandler handler, [FromBody] EditEventDto editEvent) =>
                await handler.EditEvent(editEvent));
            
        eventgroup.MapDelete(
            "{eventId:long}",
            async ([FromServices] IEventHandler handler, [FromRoute] long eventId) =>
                await handler.RemoveEvent(eventId));
    }
}