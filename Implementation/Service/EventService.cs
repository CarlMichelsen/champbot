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
    public async Task<ServiceResponse<EventDto>> AddEvent(CreateEventDto createEvent)
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
            var err = Enum.GetName(addResult.Error!.Type)!;
            return new ServiceResponse<EventDto>(err);
        }

        var mappedResult = EventMapper.MapEvent(addResult.Unwrap());
        if (mappedResult.IsError)
        {
            var err = Enum.GetName(mappedResult.Error!.Type)!;
            return new ServiceResponse<EventDto>(err);
        }

        return new ServiceResponse<EventDto>(mappedResult.Unwrap());
    }

    public async Task<ServiceResponse<EventDto>> EditEvent(EditEventDto editEvent)
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
            var err = Enum.GetName(editResult.Error!.Type)!;
            return new ServiceResponse<EventDto>(err);
        }

        var mappedResult = EventMapper.MapEvent(editResult.Unwrap());
        if (mappedResult.IsError)
        {
            var err = Enum.GetName(mappedResult.Error!.Type)!;
            return new ServiceResponse<EventDto>(err);
        }

        return new ServiceResponse<EventDto>(mappedResult.Unwrap());
    }

    public async Task<ServiceResponse<List<EventDto>>> GetEvents()
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
            var err = Enum.GetName(eventsResult.Error!.Type)!;
            return new ServiceResponse<List<EventDto>>(err);
        }

        var mappedResults = eventsResult
            .Unwrap()
            .Select(EventMapper.MapEvent)
            .ToList();

        if (mappedResults.Exists(mr => mr.IsError))
        {
            return new ServiceResponse<List<EventDto>>("map error");
        }

        var mapped = mappedResults
            .Select(mr => mr.Unwrap())
            .ToList();
        
        return new ServiceResponse<List<EventDto>>(mapped);
    }

    public async Task<ServiceResponse> RemoveEvent(long eventId)
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
            var err = Enum.GetName(deleteResult.Error!.Type)!;
            return new ServiceResponse(err);
        }

        return new ServiceResponse();
    }
}