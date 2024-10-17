using Domain.Dto.Event;
using Interface.Handler;
using Microsoft.AspNetCore.Mvc;

namespace App.Endpoints;

public static class ReminderEndpoints
{
    public static void RegisterReminderEndpoints(
        this IEndpointRouteBuilder apiGroup)
    {
        var eventgroup = apiGroup
            .MapGroup("event/reminder")
            .WithTags("Reminder");

        eventgroup.MapPost(
            string.Empty,
            async ([FromServices] IReminderHandler handler, [FromBody] CreateReminderDto createReminder) =>
                await handler.AddReminder(createReminder));
        
        eventgroup.MapPut(
            string.Empty,
            async ([FromServices] IReminderHandler handler, [FromBody] EditReminderDto editReminder) =>
                await handler.EditReminder(editReminder));
        
        eventgroup.MapDelete(
            "{eventId:long}/{reminderId:long}",
            async ([FromRoute] long eventId, [FromRoute] long reminderId, [FromServices] IReminderHandler handler) =>
                await handler.RemoveReminder(eventId, reminderId));
    }
}