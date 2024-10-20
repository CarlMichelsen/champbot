using Domain.Abstraction;
using Domain.Dto;
using Domain.Dto.Event;
using Implementation.Util;
using Interface.Accessor;
using Interface.Repository;
using Interface.Service;

namespace Implementation.Service;

public class EventService(
    IUserContextAccessor userContextAccessor,
    IEventRepository eventRepository) : IEventService
{
    public async Task<Result<ServiceResponse<EventDto>>> AddEvent(CreateEventDto createEvent)
    {
        var userResult = userContextAccessor.GetUserContext();
        if (userResult.IsError)
        {
            return new ServiceResponse<EventDto>("unauthorized");
        }

        var addResult = await eventRepository.AddEvent(
            userResult.Unwrap().User.Id,
            createEvent.EventName,
            createEvent.EventTimeUtc);

        if (addResult.IsError)
        {
            return addResult.Error!;
        }

        var mappedResult = EventMapper.MapEvent(addResult.Unwrap());
        if (mappedResult.IsError)
        {
            return mappedResult.Error!;
        }

        return new ServiceResponse<EventDto>(mappedResult.Unwrap());
    }

    public async Task<Result<ServiceResponse<EventDto>>> EditEvent(EditEventDto editEvent)
    {
        var userResult = userContextAccessor.GetUserContext();
        if (userResult.IsError)
        {
            return new ServiceResponse<EventDto>("unauthorized");
        }

        var editResult = await eventRepository.EditEvent(
            userResult.Unwrap().User.Id,
            editEvent.EventId,
            editEvent.EventTimeUtc,
            editEvent.Name);
        
        if (editResult.IsError)
        {
            return editResult.Error!;
        }

        var mappedResult = EventMapper.MapEvent(editResult.Unwrap());
        if (mappedResult.IsError)
        {
            return mappedResult.Error!;
        }

        return new ServiceResponse<EventDto>(mappedResult.Unwrap());
    }

    public async Task<Result<ServiceResponse<List<EventDto>>>> GetEvents()
    {
        var userResult = userContextAccessor.GetUserContext();
        if (userResult.IsError)
        {
            return new ServiceResponse<List<EventDto>>("unauthorized");
        }

        var eventsResult = await eventRepository
            .GetEvents(userResult.Unwrap().User.Id);
        
        if (eventsResult.IsError)
        {
            return eventsResult.Error!;
        }

        var mappedResults = eventsResult
            .Unwrap()
            .Select(EventMapper.MapEvent)
            .ToList();

        var error = mappedResults.FirstOrDefault(mr => mr.IsError);
        if (error is not null)
        {
            return error.Error!;
        }

        var mapped = mappedResults
            .Select(mr => mr.Unwrap())
            .ToList();
        
        return new ServiceResponse<List<EventDto>>(mapped);
    }

    public async Task<Result<ServiceResponse>> RemoveEvent(long eventId)
    {
        var userResult = userContextAccessor.GetUserContext();
        if (userResult.IsError)
        {
            return new ServiceResponse<EventDto>("unauthorized");
        }

        var deleteResult = await eventRepository
            .RemoveEvent(userResult.Unwrap().User.Id, eventId);

        if (deleteResult.IsError)
        {
            return deleteResult.Error!;
        }

        return new ServiceResponse();
    }
}